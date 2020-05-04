using CefSharp;
using CefSharp.Wpf;
using System.Windows;

namespace Bcfier.WebViewIntegration
{
  public partial class BrowserManager
  {
    private readonly ChromiumWebBrowser _webBrowser;

    public BrowserManager(ChromiumWebBrowser webBrowser)
    {
      _webBrowser = webBrowser;
      JavaScriptBridge.Instance.SetWebBrowser(_webBrowser);

      _webBrowser.LoadingStateChanged += (s, e) =>
      {
        if (!e.IsLoading) // Not loading means the load is complete
        {
          Application.Current.Dispatcher.Invoke(() =>
          {
            // Register the bridge between JS and C#
            // This also registers the callback that should be bound to by OpenProject to receive messages from BCFier
            _webBrowser.JavascriptObjectRepository.UnRegisterAll();
            _webBrowser.GetMainFrame().ExecuteJavaScriptAsync($"CefSharp.DeleteBoundObject('{JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME}');");
            _webBrowser.JavascriptObjectRepository.Register(JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME, new BcfierJavascriptInterop(), true);

            _webBrowser.GetMainFrame().ExecuteJavaScriptAsync(@"(async function(){
await CefSharp.BindObjectAsync(""" + JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME + @""", ""bound"");
window." + JavaScriptBridge.REVIT_BRIDGE_JAVASCRIPT_NAME + @".sendMessageToOpenProject = (message) => {console.log(JSON.parse(message))}; // This is the callback to be used by OpenProject for receiving messages
window.dispatchEvent(new Event('" + JavaScriptBridge.REVIT_READY_EVENT_NAME + @"'));
})();");
          });
          // Now in JS, call this: openProjectBridge.messageFromOpenProject('Message from JS');
        }
      };

      _webBrowser.IsBrowserInitializedChanged += (s, e) => { _webBrowser.ShowDevTools(); };
    }
  }
}
