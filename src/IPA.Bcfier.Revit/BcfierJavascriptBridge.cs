using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IPA.Bcfier.Models.Settings;
using IPA.Bcfier.Services;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using IPA.Bcfier.Revit.Models;
using IPA.Bcfier.Models.Bcf;

namespace IPA.Bcfier.Revit
{
    public class BcfierJavascriptBridge
    {
        private readonly RevitTaskQueueHandler _revitTaskQueueHandler;

        public BcfierJavascriptBridge(RevitTaskQueueHandler revitTaskQueueHandler)
        {
            _revitTaskQueueHandler = revitTaskQueueHandler;
        }

        private class DataClass
        {
            public string Command { get; set; }

            public string Data { get; set; }
        }
        public async Task SendDataToRevit(string data, IJavascriptCallback javascriptCallback)
        {
            var classData = JsonConvert.DeserializeObject<DataClass>(data)!;

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };

            if (classData.Command == "getSettings")
            {
                var userSettings = await new IPA.Bcfier.Services.SettingsService().LoadSettingsAsync();
                await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(userSettings, serializerSettings));
            }
            else if (classData.Command == "setSettings")
            {
                var userSettings = JsonConvert.DeserializeObject<Settings>(classData.Data);
                await new IPA.Bcfier.Services.SettingsService().SaveSettingsAsync(userSettings!);
                await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(userSettings, serializerSettings));
            }
            else if (classData.Command == "openDocumentation")
            {
                System.Diagnostics.Process.Start("https://docs.dangl-it.com/Projects/IPA.BCFier");
                await javascriptCallback.ExecuteAsync();
            }
            else if (classData.Command == "importBcfFile")
            {
                // Since we need a Revit context (more specifically access to the UI thread), 
                // we're enqueuing that task to be executed in the Revit context
                _revitTaskQueueHandler.OpenBcfFileCallbacks.Enqueue(javascriptCallback);
            }
            else if (classData.Command == "exportBcfFile")
            {
                _revitTaskQueueHandler.SaveBcfFileCallbacks.Enqueue(new SaveBcfFileQueueItem
                {
                    Callback = javascriptCallback,
                    BcfFile = JsonConvert.DeserializeObject<BcfFile>(classData.Data)
                });
            }
            else if (classData.Command == "createViewpoint")
            {
                _revitTaskQueueHandler.CreateRevitViewpointCallbacks.Enqueue(javascriptCallback);
            }
            else
            {
                // TODO return error for unrecognized commands
            }
        }
    }
}
