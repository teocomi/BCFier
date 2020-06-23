using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OpenProject.Revit.Data;
using OpenProject.Shared.ViewModels.Bcf;
using OpenProject.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZetaIpc.Runtime.Client;
using ZetaIpc.Runtime.Server;
using ZetaIpc.Runtime.Helper;

namespace OpenProject.Revit.Entry
{
  public class BcfierIpcHandler
  {
    private readonly UIApplication _uiapp;
    private Action<string> _sendData;
    private static readonly object _callbackStackLock = new object();
    private static readonly Stack<Action> _callbackStack = new Stack<Action>();

    public BcfierIpcHandler(UIApplication uiapp)
    {
      _uiapp = uiapp ?? throw new ArgumentNullException(nameof(uiapp));

      uiapp.Idling += (s, e) =>
      {
        lock (_callbackStackLock)
        {
          if (_callbackStack.Any())
          {
            var action = _callbackStack.Pop();
            action.Invoke();
          }
        }
      };
    }

    public int StartLocalServerAndReturnPort()
    {
      var freePort = FreePortHelper.GetFreePort();
      var server = new IpcServer();
      server.Start(freePort);
      server.ReceivedRequest += (sender, e) =>
      {
        var eventArgs = JsonConvert.DeserializeObject<WebUIMessageEventArgs>(e.Request);
        var localMessageType = eventArgs.MessageType;
        var localTrackingId = eventArgs.TrackingId;
        var localMessagePayload = eventArgs.MessagePayload;
        _callbackStack.Push(() =>
        {
          if (localMessageType == MessageTypes.VIEWPOINT_DATA)
          {
            var bcfViewpoint = MessageDeserializer.DeserializeBcfViewpoint(new WebUIMessageEventArgs(localMessageType, localTrackingId, localMessagePayload));
            OpenView(bcfViewpoint);
          }
          if (localMessageType == MessageTypes.VIEWPOINT_GENERATION_REQUESTED)
          {
            AddView(localTrackingId);
          }
        });
      };

      return freePort;
    }

    public void StartLocalClient(int bcfierWinServerPort)
    {
      var client = new IpcClient();
      client.Initialize(bcfierWinServerPort);
      _sendData = (message) =>
      {
        client.Send(message);
      };
    }

    /// <summary>
    /// Raises the External Event to accomplish a transaction in a modeless window
    /// http://help.autodesk.com/view/RVT/2014/ENU/?guid=GUID-0A0D656E-5C44-49E8-A891-6C29F88E35C0
    /// http://matteocominetti.com/starting-a-transaction-from-an-external-application-running-outside-of-api-context-is-not-allowed/
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenView(BcfViewpointViewModel view)
    {
      try
      {
        UIDocument uidoc = _uiapp.ActiveUIDocument;

        if (uidoc.ActiveView.ViewType == ViewType.Schedule)
        {
          MessageBox.Show("BCFier can't take snapshots of schedules.",
              "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }

        ExtEvntOpenView.ShowBcfViewpoint(_uiapp, view);
      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error opening a View!", "exception: " + ex1);
      }
    }

    /// <summary>
    /// Same as in the windows app, but here we generate a VisInfo that is attached to the view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddView(string trackingId)
    {
      try
      {
        var generatedViewpoint = RevitView.GenerateViewpoint(_uiapp.ActiveUIDocument);
        var snapshot = GetRevitSnapshot(_uiapp.ActiveUIDocument.Document);
        var messageContent = new ViewpointGeneratedApiMessage
        {
          SnapshotPngBase64 = "data:image/png;base64," + ConvertToBase64(snapshot),
          Viewpoint = MessageSerializer.SerializeBcfViewpoint(generatedViewpoint)
        };

        var serializerSettings = new JsonSerializerSettings
        {
          ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var jsonPayload = JObject.Parse(JsonConvert.SerializeObject(messageContent.Viewpoint.Viewpoint, serializerSettings));
        jsonPayload["components"] = JObject.Parse(JsonConvert.SerializeObject(messageContent.Viewpoint.Components, serializerSettings));
        jsonPayload["snapshot"] = messageContent.SnapshotPngBase64;
        //var payloadString = JsonConvert.SerializeObject(messageContent);
        var payloadString = jsonPayload.ToString();

        var eventArgs = new WebUIMessageEventArgs(MessageTypes.VIEWPOINT_GENERATED, trackingId, payloadString);
        var jsonEventArgs = JsonConvert.SerializeObject(eventArgs);
        _sendData(jsonEventArgs);
      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error adding a View!", "exception: " + ex1);
      }
    }

    private Stream GetRevitSnapshot(Document doc)
    {
      try
      {
        var tempPath = Path.Combine(Path.GetTempPath(), "BCFier");
        Directory.CreateDirectory(tempPath);
        string tempImg = Path.Combine(tempPath, Path.GetTempFileName() + ".png");
        var options = new ImageExportOptions
        {
          FilePath = tempImg,
          HLRandWFViewsFileType = ImageFileType.PNG,
          ShadowViewsFileType = ImageFileType.PNG,
          ExportRange = ExportRange.VisibleRegionOfCurrentView,
          ZoomType = ZoomFitType.FitToPage,
          ImageResolution = ImageResolution.DPI_72,
          PixelSize = 1000
        };
        doc.ExportImage(options);

        var memStream = new MemoryStream();
        using (var fs = System.IO.File.OpenRead(tempImg))
        {
          fs.CopyTo(memStream);
        }
        File.Delete(tempImg);

        memStream.Position = 0;
        return memStream;
      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error!", "exception: " + ex1);
        throw;
      }
    }

    private static string ConvertToBase64(Stream stream)
    {
      byte[] bytes;
      using (var memoryStream = new MemoryStream())
      {
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();
      }

      return Convert.ToBase64String(bytes);
    }
  }
}