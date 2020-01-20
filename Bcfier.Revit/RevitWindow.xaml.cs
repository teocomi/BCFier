using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bcfier.Revit.Data;
using Bcfier.Revit.Entry;
using System.ComponentModel;
using System.Threading.Tasks;
using Bcfier.Data.Utils;
using System.IO;
using Bcfier.ViewModels.Bcf;
using Bcfier.WebViewIntegration;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Bcfier.Revit
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class RevitWindow : Window
  {
    private ExternalEvent ExtEvent;
    private ExtEvntOpenView Handler;
    private UIApplication uiapp;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="_uiapp"></param>
    /// <param name="exEvent"></param>
    /// <param name="handler"></param>
    public RevitWindow(UIApplication _uiapp, ExternalEvent exEvent, ExtEvntOpenView handler)
    {
      InitializeComponent();

      try
      {
        ExtEvent = exEvent;
        Handler = handler;
        uiapp = _uiapp;
      }
      catch (Exception ex1)
      {
        TaskDialog.Show("Error!", "exception: " + ex1);
      }

      // Notify OpenProject about the current document
      var uidoc = uiapp.ActiveUIDocument;
      JavaScriptBridge.Instance
        .SendMessageToOpenProject(MessageTypes.REVIT_LOADED, string.Empty, JsonConvert.SerializeObject(new
        {
          uidoc.Document.Title
        }));

      var callbackStackLock = new object();
      var callbackStack = new Stack<Action>();

      uiapp.Idling += (s, e) =>
        {
          lock (callbackStackLock)
          {
            if (callbackStack.Any())
            {
              var action = callbackStack.Pop();
              action.Invoke();
            }
          }
        };

      JavaScriptBridge
        .Instance
        .OnWebUIMessageReveived += (s, e) =>
          {
            var localS = s;
            var localE = e;
            callbackStack.Push(() =>
            {
              if (localE.MessageType == MessageTypes.VIEWPOINT_DATA)
              {
                var bcfViewpoint = MessageDeserializer.DeserializeBcfViewpoint(localE);
                OpenView(bcfViewpoint);
              }
              if (localE.MessageType == MessageTypes.VIEWPOINT_GENERATION_REQUESTED)
              {
                AddView(localE.TrackingId);
              }
            });
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
        UIDocument uidoc = uiapp.ActiveUIDocument;

        if (uidoc.ActiveView.ViewType == ViewType.Schedule)
        {
          MessageBox.Show("BCFier can't take snapshots of schedules.",
              "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }

        if (uidoc.ActiveView.ViewType == ViewType.ThreeD)
        {
          var view3D = (View3D)uidoc.ActiveView;
          if (view3D.IsPerspective)
          {
            MessageBox.Show("This operation is not allowed in a Perspective View.\nPlease close the current window(s) and retry.",
                "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
          }

        }
        Handler.v = view;
        ExtEvent.Raise();
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
        var generatedViewpoint = RevitView.GenerateViewpoint(uiapp.ActiveUIDocument);
        var snapshot = GetRevitSnapshot(uiapp.ActiveUIDocument.Document);
        var messageContent = new ViewpointGeneratedApiMessage
        {
          SnapshotPngBase64 = ConvertToBase64(snapshot),
          Viewpoint = MessageSerializer.SerializeBcfViewpoint(generatedViewpoint)
        };

        var serializerSettings = new JsonSerializerSettings
        {
          ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var jsonPayload = JObject.Parse(JsonConvert.SerializeObject(messageContent.Viewpoint.Viewpoint,serializerSettings));
        jsonPayload["components"] = JObject.Parse(JsonConvert.SerializeObject(messageContent.Viewpoint.Components, serializerSettings));
        jsonPayload["snapshot"] = messageContent.SnapshotPngBase64;
        //var payloadString = JsonConvert.SerializeObject(messageContent);
        var payloadString = jsonPayload.ToString();
        JavaScriptBridge.Instance.SendMessageToOpenProject(MessageTypes.VIEWPOINT_GENERATED, trackingId, payloadString);
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

    #region private methods

    /// <summary>
    /// passing event to the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      // e.Cancel = Bcfier.onClosing(e);
    }
    #endregion

    //stats
    private void RevitWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      Task.Run(() =>
      {
        StatHat.Post.EzCounter(@"hello@teocomi.com", "BCFierRevitStart", 1);
      });
    }

  }
}