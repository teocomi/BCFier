using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IPA.Bcfier.Models.Settings;

namespace IPA.Bcfier.Revit
{
    public class BcfierJavascriptBridge
    {
        private class DataClass
        {
            public string Command { get; set; }

            public string Data { get; set; }
        }

        public async Task SendDataToRevit(string data, IJavascriptCallback javascriptCallback)
        {
            var classData = JsonConvert.DeserializeObject<DataClass>(data)!;

            DefaultContractResolver contractResolver = new DefaultContractResolver
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
                var userSettings = await new Services.SettingsService().LoadSettingsAsync();
                await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(userSettings, serializerSettings));
            }
            else if (classData.Command == "setSettings")
            {
                var userSettings = JsonConvert.DeserializeObject<Settings>(classData.Data);
                await new Services.SettingsService().SaveSettingsAsync(userSettings!);
                await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(userSettings, serializerSettings));
            }
            else if (classData.Command == "openDocumentation")
            {
                System.Diagnostics.Process.Start("https://docs.dangl-it.com/Projects/IPA.BCFier");
                await javascriptCallback.ExecuteAsync();
            }
            else
            {
                // TODO return error for unrecognized commands
            }
        }
    }
}
