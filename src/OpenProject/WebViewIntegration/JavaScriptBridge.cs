using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using OpenProject.Shared;
using System.Windows;

namespace OpenProject.WebViewIntegration
{
  public class JavaScriptBridge
  {
    /// <summary>
    /// This is the name of the global window object that's set in JavaScript, e.g.
    /// 'window.RevitBridge'.
    /// </summary>
    public const string REVIT_BRIDGE_JAVASCRIPT_NAME = "RevitBridge";

    public const string REVIT_READY_EVENT_NAME = "revit.plugin.ready";

    private JavaScriptBridge()
    {
      // This class should only be available as a singleton, access
      // via the static 'Instance' property.
    }

    private ChromiumWebBrowser _webBrowser;
    public static JavaScriptBridge Instance { get; } = new JavaScriptBridge();

    public event WebUIMessageReceivedEventHandler OnWebUIMessageReveived;

    public delegate void WebUIMessageReceivedEventHandler(object sender, WebUIMessageEventArgs e);

    private void ChangeLoadingState(object sender, object eventArgs)
    {
      isLoaded = true;
    }

    public void SetWebBrowser(ChromiumWebBrowser webBrowser)
    {
      if (_webBrowser != null)
      {
        _webBrowser.LoadingStateChanged -= ChangeLoadingState;
      }

      _webBrowser = webBrowser;
      _webBrowser.LoadingStateChanged += ChangeLoadingState;
    }

    private bool isLoaded = false;

    public void SendMessageToRevit(string messageType, string trackingId, string messagePayload)
    {
      if (!isLoaded)
      {
        return;
      }

      if (messageType == MessageTypes.INSTANCE_SELECTED)
      {
        // This is the case at the beginning when the user selects which instance of OpenProject
        // should be accessed. We're not relaying this to Revit.
        HandleInstanceNameReceived(messagePayload);
      }
      else if (messageType == MessageTypes.LAST_INSTANCE_REQUESTED)
      {
        var lastInstanceUrl = ConfigurationHandler.LoadBcfierBrowserInitialAddressOrNull();
        SendMessageToOpenProject(MessageTypes.LAST_INSTANCE, trackingId, lastInstanceUrl);
      }
      else if (messageType == MessageTypes.FOCUS_REVIT_APPLICATION)
      {
        RevitMainWindowHandler.SetFocusToRevit();
      }
      else
      {
        var eventArgs = new WebUIMessageEventArgs(messageType, trackingId, messagePayload);
        OnWebUIMessageReveived?.Invoke(this, eventArgs);
        // For some UI operations, revit should be focused
        RevitMainWindowHandler.SetFocusToRevit();
      }
    }

    public void SendMessageToOpenProject(string messageType, string trackingId, string messagePayload)
    {
      if (!isLoaded)
      {
        return;
      }

      if (messageType == MessageTypes.CLOSE_DESKTOP_APPLICATION)
      {
        // This message means we should exit the application
        System.Environment.Exit(0);
        return;
      }

      var messageData = JsonConvert.SerializeObject(new { messageType, trackingId, messagePayload });
      var encodedMessage = JsonConvert.ToString(messageData);
      Application.Current.Dispatcher.Invoke(() =>
      {
        _webBrowser?.GetMainFrame()
          .ExecuteJavaScriptAsync($"{REVIT_BRIDGE_JAVASCRIPT_NAME}.sendMessageToOpenProject({encodedMessage})");
      });
    }

    private void HandleInstanceNameReceived(string instanceName)
    {
      var urlToOpen = string.Empty;
      if (instanceName.Contains("."))
      {
        // It's likely an absolute url
        urlToOpen = instanceName;
      }
      else
      {
        // It's an OpenProject team / organization
        urlToOpen = $"https://{instanceName}.openproject.com";
      }

      ConfigurationHandler.SaveInitialBrowserAddress(urlToOpen);

      Application.Current.Dispatcher.Invoke(() =>
      {
        _webBrowser.Address = urlToOpen;
      });
    }
  }
}
