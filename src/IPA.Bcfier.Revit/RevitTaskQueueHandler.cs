using Autodesk.Revit.UI.Events;
using CefSharp;
using IPA.Bcfier.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IPA.Bcfier.Revit
{
    public class RevitTaskQueueHandler
    {
        public Queue<IJavascriptCallback> OpenBcfFileCallbacks = new Queue<IJavascriptCallback>();

        public void OnIdling(object sender, IdlingEventArgs args)
        {
            if (OpenBcfFileCallbacks.Count > 0)
            {
                var callback = OpenBcfFileCallbacks.Dequeue();
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
        }
    }
}
