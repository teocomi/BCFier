using OpenProject.Shared;
using Newtonsoft.Json;
using ZetaIpc.Runtime.Client;
using ZetaIpc.Runtime.Server;

namespace OpenProject.WebViewIntegration
{
  public static class IpcManager
  {
    public static void StartIpcCommunication(int serverPort, int clientPort)
    {
      var server = new IpcServer();
      server.Start(serverPort);
      server.ReceivedRequest += (sender, e) =>
      {
        var deserialized = JsonConvert.DeserializeObject<WebUIMessageEventArgs>(e.Request);
        JavaScriptBridge.Instance
        .SendMessageToOpenProject(deserialized.MessageType, deserialized.TrackingId, deserialized.MessagePayload);
      };

      var client = new IpcClient();
      client.Initialize(clientPort);
      JavaScriptBridge.Instance
        .OnWebUIMessageReveived += (s, e) => client.Send(JsonConvert.SerializeObject(e));
    }
  }
}
