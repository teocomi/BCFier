using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bcfier.WebViewIntegration
{
  public partial class BrowserManager
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
            _webBrowser.JavascriptObjectRepository.Register(JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME, new BcfierJavascriptInterop(), true);
            _webBrowser.GetMainFrame().ExecuteJavaScriptAsync(@"(async function(){
await CefSharp.BindObjectAsync(""" + JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME + @""", ""bound"");
window." + JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME + @".sendMessageToOpenProject = (message) => {console.log(JSON.parse(message))}; // This is the callback to be used by OpenProject for receiving messages
})();");
            _webBrowser.GetMainFrame().ExecuteJavaScriptAsync($"window.dispatchEvent(new Event('{JavaScriptBridge.REVIT_READY_EVENT_NAME}'));");
            // Now in JS, call this: openProjectBridge.messageFromOpenProject('Message from JS');
          }
        }
      };

      // TODO CHECK IF COOKIES AND LOCALSTORAGE CAN BE PERSISTED VIA C#
      // TODO -> SAVE LAST LOCATION FROM WEBVIEW AND REOPEN

      _webBrowser.Initialized += (s, e) => _webBrowser.ShowDevTools();
    }
  }
}
