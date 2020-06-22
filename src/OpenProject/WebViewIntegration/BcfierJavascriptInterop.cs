namespace OpenProject.WebViewIntegration
{
  public partial class BrowserManager
  {
    public class BcfierJavascriptInterop
    {
      private readonly JavaScriptBridge _javaScriptBridge = JavaScriptBridge.Instance;

      public void SendMessageToRevit(string type, string trackingId, string payload)
      {
        _javaScriptBridge.SendMessageToRevit(type, trackingId, payload);
      }
    }
  }
}
