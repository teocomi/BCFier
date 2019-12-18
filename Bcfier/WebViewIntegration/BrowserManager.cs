using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bcfier.WebViewIntegration
{
  public class BrowserManager
  {
    private readonly ChromiumWebBrowser _webBrowser;

    public BrowserManager(ChromiumWebBrowser webBrowser)
    {
      _webBrowser = webBrowser;

      var hasGreetedLock = new object();
      var hasGreeted = false;
      _webBrowser.FrameLoadEnd += (s, e) =>
      {
        lock (hasGreetedLock)
        {
          if (!hasGreeted)
          {
            hasGreeted = true;
            // Execute JS from C#
            _webBrowser.ShowDevTools();

            // Register the bridge between JS and C#
            // This also registers the callback that should be bound to by OpenProject to receive messages from BCFier
            _webBrowser.JavascriptObjectRepository.Register("RevitBridge", new BcfierJavascriptInterop(), true);
            _webBrowser.GetMainFrame().ExecuteJavaScriptAsync(@"(async function(){
await CefSharp.BindObjectAsync(""RevitBridge"", ""bound"");
window.RevitBridge.sendMessageToOpenProject = (message) => {console.log(JSON.parse(message))}; // This is the callback to be used by OpenProject for receiving messages
})();");
            _webBrowser.GetMainFrame().ExecuteJavaScriptAsync("window.dispatchEvent(new Event('revit.plugin.ready'));");

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new DefaultContractResolver
            {
              NamingStrategy = new CamelCaseNamingStrategy()
            };

            JavaScriptBridge.Instance.OnWebUIMessageSent += (sender, eventArgs) =>
              {
                var messageData = JsonConvert.SerializeObject(new { eventArgs.MessageType, eventArgs.TrackingId, eventArgs.MessagePayload }, jsonSerializerSettings)
                  .Replace("\"", "\\\"");
                _webBrowser.GetMainFrame()
                  .ExecuteJavaScriptAsync($"bcfierBridge.sendMessageToOpenProject(\"{messageData}\")");
              };


            var myFunkyTimer = new System.Timers.Timer(3000);
            myFunkyTimer.Elapsed += (s2, e2) =>
            {
              JavaScriptBridge.Instance.SendMessageToOpenProject("FunkyType", "FunkyId", "Hello World!");
              myFunkyTimer.Stop();
            };
            myFunkyTimer.Start();


            // Now in JS, call this: openProjectBridge.messageFromOpenProject('Message from JS');
          }
        }
      };

      // TODO CHECK IF COOKIES AND LOCALSTORAGE CAN BE PERSISTED VIA C#
      // TODO -> SAVE LAST LOCATION FROM WEBVIEW AND REOPEN

      _webBrowser.Initialized += (s, e) => _webBrowser.ShowDevTools();
    }

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
