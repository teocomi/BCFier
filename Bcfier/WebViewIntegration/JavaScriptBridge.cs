namespace Bcfier.WebViewIntegration
{
  public class JavaScriptBridge
  {
    private JavaScriptBridge()
    {
      // This class should only be available as a singleton, access
      // via the static 'Instance' property.
    }

    public static JavaScriptBridge Instance { get; } = new JavaScriptBridge();

    public event WebUIMessageReceivedEventHandler OnWebUIMessageReveived;
    public delegate void WebUIMessageReceivedEventHandler(object sender, WebUIMessageEventArgs e);

    public event WebUIMessageSentEventHandler OnWebUIMessageSent;
    public delegate void WebUIMessageSentEventHandler(object sender, WebUIMessageEventArgs e);

    public void SendMessageToBcfier(string messageType, string trackingId, string messagePayload)
    {
      var eventArgs = new WebUIMessageEventArgs(messageType, trackingId, messagePayload);
      OnWebUIMessageReveived?.Invoke(this, eventArgs);
    }

    public void SendMessageToOpenProject(string messageType, string trackingId, string messagePayload)
    {
      var eventArgs = new WebUIMessageEventArgs(messageType, trackingId, messagePayload);
      OnWebUIMessageSent?.Invoke(this, eventArgs);
    }
  }
}
