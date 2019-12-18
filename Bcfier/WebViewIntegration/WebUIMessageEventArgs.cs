using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bcfier.WebViewIntegration
{
  public class WebUIMessageEventArgs : EventArgs
  {
    public WebUIMessageEventArgs(string messageType,
      string trackingId,
      string messagePayload)
    {
      MessageType = messageType;
      trackingId = TrackingId;
      MessagePayload = messagePayload;
    }

    public string MessageType { get; }
    public string MessagePayload { get; }
    public string TrackingId { get; }
  }
}
