using Autodesk.Revit.UI.Events;
using CefSharp;
using IPA.Bcfier.Revit.Models;
using IPA.Bcfier.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IPA.Bcfier.Revit
{
    public class RevitTaskQueueHandler
    {
        public Queue<IJavascriptCallback> OpenBcfFileCallbacks = new Queue<IJavascriptCallback>();
        public Queue<SaveBcfFileQueueItem> SaveBcfFileCallbacks = new Queue<SaveBcfFileQueueItem>();

        public void OnIdling(object sender, IdlingEventArgs args)
        {
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
    }
}
