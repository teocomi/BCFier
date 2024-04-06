using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using CefSharp;
using IPA.Bcfier.Models.Bcf;
using IPA.Bcfier.Revit.Models;
using IPA.Bcfier.Revit.Services;
using IPA.Bcfier.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IPA.Bcfier.Revit
{
    public class RevitTaskQueueHandler
    {
        public Queue<IJavascriptCallback> OpenBcfFileCallbacks { get; } = new Queue<IJavascriptCallback>();
        public Queue<SaveBcfFileQueueItem> SaveBcfFileCallbacks { get; } = new Queue<SaveBcfFileQueueItem>();
        public Queue<IJavascriptCallback> CreateRevitViewpointCallbacks { get; } = new Queue<IJavascriptCallback>();
        public Queue<ShowViewpointQueueItem> ShowViewpointQueueItems { get; } = new Queue<ShowViewpointQueueItem>();
        private bool shouldUnregister = false;

        private Queue<ViewContinuationInstructions> AfterViewCreationCallbackQueue { get; } = new Queue<ViewContinuationInstructions>();

        public void OnIdling(object sender, IdlingEventArgs args)
        {
            var uiApplication = sender as UIApplication;
            if (uiApplication == null)
            {
                return;
            }

            if (shouldUnregister)
            {
                uiApplication.Idling -= OnIdling;
            }

            if (OpenBcfFileCallbacks.Count > 0)
            {
                var callback = OpenBcfFileCallbacks.Dequeue();
                HandleOpenBcfFileCallback(callback);
            }

            if (SaveBcfFileCallbacks.Count > 0)
            {
                var saveBcfFileQueueItem = SaveBcfFileCallbacks.Dequeue();
                HandleSaveBcfFileCallback(saveBcfFileQueueItem);
            }

            if (CreateRevitViewpointCallbacks.Count > 0)
            {
                var uiDocument = uiApplication.ActiveUIDocument;
                var callback = CreateRevitViewpointCallbacks.Dequeue();
                HandleCreateRevitViewpointCallback(callback, uiDocument);
            }

            if (ShowViewpointQueueItems.Count > 0)
            {
                var uiDocument = uiApplication.ActiveUIDocument;
                var showViewpointQueueItem = ShowViewpointQueueItems.Dequeue();
                HandleShowRevitViewpointCallback(showViewpointQueueItem.Callback, showViewpointQueueItem.Viewpoint, uiDocument);
            }

            if (AfterViewCreationCallbackQueue.Count > 0)
            {
                var uiDocument = uiApplication.ActiveUIDocument;
                HandlAfterViewCreationCallbackQueueItems(uiDocument);
            }
        }

        private void HandlAfterViewCreationCallbackQueueItems(UIDocument uiDocument)
        {
            // This is pretty complicated. The signal flow is like this:
            // 1. User clicks on a button in the web view
            // 2. We send that data to the Revit API, which puts the request on a queue
            // 3. During the Revit Application.Idling event, we process the queue
            // 4. A viewpoint display request is processed, and a view is created and set as active view
            //    The active view can only be set in an asynchronous way from the Application.Idling event in
            //    the Revit API, so we need to wait until the new view is loaded
            // 5. Once the view is loaded, we check this other queue here and apply the callback, which sets
            //    e.g. the selected components
            // 6. After that, we can inform the frontend
            var queueLength = AfterViewCreationCallbackQueue.Count;
            for (var i = 0; i < queueLength; i++)
            {
                var item = AfterViewCreationCallbackQueue.Dequeue();
                if (item?.ViewId == uiDocument.ActiveView.Id)
                {
                    item.ViewContinuation?.Invoke();
                    Task.Run(async () =>
                    {
                        await item.JavascriptCallback.ExecuteAsync();
                    });
                }
                else if (item != null)
                {
                    AfterViewCreationCallbackQueue.Enqueue(item);
                }
            }
        }

        public void UnregisterEventHandler()
        {
            shouldUnregister = true;
        }

        private void HandleOpenBcfFileCallback(IJavascriptCallback callback)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "BCF Files (*.bcf, *.bcfzip)|*.bcf;*.bcfzip"
            };

            if (!openFileDialog.ShowDialog() ?? false || openFileDialog.FileName == null)
            {
                return;
            }

            var bcfFilePath = openFileDialog.FileName;
            Task.Run(async () =>
            {
                var bcfFileName = Path.GetFileName(bcfFilePath);
                using var bcfFileStream = File.OpenRead(bcfFilePath);
                var bcfResult = await new BcfImportService().ImportBcfFileAsync(bcfFileStream, bcfFileName ?? "issue.bcf");
                var contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                };
                await callback.ExecuteAsync(JsonConvert.SerializeObject(bcfResult, serializerSettings));
            });
        }

        private void HandleSaveBcfFileCallback(SaveBcfFileQueueItem saveBcfFileQueueItem)
        {
            if (saveBcfFileQueueItem.BcfFile == null || saveBcfFileQueueItem.Callback == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "BCF Files (*.bcf)|*.bcf"
            };

            if (!saveFileDialog.ShowDialog() ?? false || saveFileDialog.FileName == null)
            {
                return;
            }

            var bcfFilePath = saveFileDialog.FileName;
            Task.Run(async () =>
            {
                var bcfFileResult = new BcfExportService().ExportBcfFile(saveBcfFileQueueItem.BcfFile);
                using var fs = File.Create(bcfFilePath);
                await bcfFileResult.CopyToAsync(fs);

                await saveBcfFileQueueItem.Callback.ExecuteAsync();
            });
        }

        private void HandleCreateRevitViewpointCallback(IJavascriptCallback callback, UIDocument uiDocument)
        {
            var viewpointService = new RevitViewpointCreationService(uiDocument);
            var viewpoint = viewpointService.GenerateViewpoint();
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
            Task.Run(async () =>
            {
                await callback.ExecuteAsync(JsonConvert.SerializeObject(viewpoint, serializerSettings));
            });
        }

        private void HandleShowRevitViewpointCallback(IJavascriptCallback? callback, BcfViewpoint? viewpoint, UIDocument uiDocument)
        {
            if (callback == null || viewpoint == null)
            {
                return;
            }

            var viewpointService = new RevitViewpointDisplayService(uiDocument);
            var afterViewInitCallback = viewpointService.DisplayViewpoint(viewpoint);
            if (afterViewInitCallback?.ViewId == null)
            {
                Task.Run(async () =>
                {
                    await callback.ExecuteAsync();
                });
                return;
            }

            afterViewInitCallback.JavascriptCallback = callback;
            AfterViewCreationCallbackQueue.Enqueue(afterViewInitCallback);
        }
    }
}
