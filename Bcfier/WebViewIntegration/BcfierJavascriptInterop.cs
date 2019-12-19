namespace Bcfier.WebViewIntegration
{
  public partial class BrowserManager
  {
    public class BcfierJavascriptInterop
    {
      private readonly JavaScriptBridge _javaScriptBridge = JavaScriptBridge.Instance;

      public void SendMessageToBcfier(string type, string trackingId, string payload)
      {
        _javaScriptBridge.SendMessageToBcfier(type, trackingId, payload);
      }
    }
  }
}
