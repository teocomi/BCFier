using System;

namespace OpenProject.Shared
{
  public class WebUIMessageEventArgs : EventArgs
  {
    public WebUIMessageEventArgs(string messageType,
      string trackingId,
      string messagePayload)
    {
      MessageType = messageType;
      TrackingId = trackingId;
      MessagePayload = messagePayload;
    }

    public string MessageType { get; }
    public string MessagePayload { get; }
    public string TrackingId { get; }
  }
}
