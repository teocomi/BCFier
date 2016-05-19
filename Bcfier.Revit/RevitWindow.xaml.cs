using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Bcfier.Bcf.Bcf2;
using Bcfier.Revit.Data;
using Bcfier.Revit.Entry;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.ComponentModel;
using System.Threading.Tasks;
using Component = Bcfier.Bcf.Bcf2.Component;
using Point = Bcfier.Bcf.Bcf2.Point;

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
    }

    #region commands
    /// <summary>
    /// Raises the External Event to accomplish a transaction in a modeless window
    /// http://help.autodesk.com/view/RVT/2014/ENU/?guid=GUID-0A0D656E-5C44-49E8-A891-6C29F88E35C0
    /// http://matteocominetti.com/starting-a-transaction-from-an-external-application-running-outside-of-api-context-is-not-allowed/
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOpenView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        if (Bcfier.SelectedBcf() == null)
          return;
        var view = e.Parameter as ViewPoint;
        if (view == null)
          return;
        UIDocument uidoc = uiapp.ActiveUIDocument;

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
        Handler.v = view.VisInfo;
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
    private void OnAddView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {

        if (Bcfier.SelectedBcf() == null)
          return;
        var issue = e.Parameter as Markup;
        if (issue == null)
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        var dialog = new AddViewRevit(issue, Bcfier.SelectedBcf().TempPath, uiapp.ActiveUIDocument.Document);
        dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        dialog.ShowDialog();
        if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
        {
          //generate and set set the VisInfo
          issue.Viewpoints.Last().VisInfo = GenerateViewpoint();

          //get filename
          UIDocument uidoc = uiapp.ActiveUIDocument;

          if(uidoc.Document.Title!=null)
            issue.Header[0].Filename = uidoc.Document.Title;
          else
            issue.Header[0].Filename = "Unknown";
        
          Bcfier.SelectedBcf().HasBeenSaved = false;
        }

      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error adding a View!", "exception: " + ex1);
      }
    }
    #endregion

    #region private methods
    //<summary>
    //Generate a VisualizationInfo of the current view
    //</summary>
    //<returns></returns>
    private VisualizationInfo GenerateViewpoint()
    {
      try
      {
        var uidoc = uiapp.ActiveUIDocument;
        var doc = uidoc.Document;

        var v = new VisualizationInfo();

        //Corners of the active UI view
        var topLeft = uidoc.GetOpenUIViews()[0].GetZoomCorners()[0];
        var bottomRight = uidoc.GetOpenUIViews()[0].GetZoomCorners()[1];

        //It's a 2D view
        //not supported by BCF, but I store it under a custom 
        //fields using 2D coordinates and sheet id
        if (uidoc.ActiveView.ViewType != ViewType.ThreeD)
        {
          v.SheetCamera = new SheetCamera
          {
            SheetID = uidoc.ActiveView.Id.IntegerValue,
            TopLeft = new Point { X = topLeft.X, Y = topLeft.Y, Z = topLeft.Z },
            BottomRight = new Point { X = bottomRight.X, Y = bottomRight.Y, Z = bottomRight.Z }
          };
        }
        //It's a 3d view
        else
        {
          var viewCenter = new XYZ();
          var view3D = (View3D)uidoc.ActiveView;
          double zoomValue = 1;
          // it is a orthogonal view
          if (!view3D.IsPerspective)
          {
            double x = (topLeft.X + bottomRight.X) / 2;
            double y = (topLeft.Y + bottomRight.Y) / 2;
            double z = (topLeft.Z + bottomRight.Z) / 2;
            //center of the UI view
            viewCenter = new XYZ(x, y, z);

            //vector going from BR to TL
            XYZ diagVector = topLeft.Subtract(bottomRight);
            //lenght of the vector
            double dist = topLeft.DistanceTo(bottomRight) / 2;

            //ViewToWorldScale value
            zoomValue = dist * Math.Sin(diagVector.AngleTo(view3D.RightDirection)).ToMeters();

            // **** CUSTOM VALUE FOR TEKLA **** //
            // calculated sperimentally, not sure why but it works
            //if (UserSettings.Get("optTekla") == "1")
            //  zoomValue = zoomValue * 2.5;
            // **** CUSTOM VALUE FOR TEKLA **** //

            ViewOrientation3D t = RevitUtils.ConvertBasePoint(doc, viewCenter, uidoc.ActiveView.ViewDirection,
            uidoc.ActiveView.UpDirection, false);

            XYZ c = t.EyePosition;
            XYZ vi = t.ForwardDirection;
            XYZ up = t.UpDirection;


            v.OrthogonalCamera = new OrthogonalCamera
            {
              CameraViewPoint =
              {
                X = c.X.ToMeters(),
                Y = c.Y.ToMeters(),
                Z = c.Z.ToMeters()
              },
              CameraUpVector =
              {
                X = up.X.ToMeters(),
                Y = up.Y.ToMeters(),
                Z = up.Z.ToMeters()
              },
              CameraDirection =
              {
                X = vi.X.ToMeters() * -1,
                Y = vi.Y.ToMeters() * -1,
                Z = vi.Z.ToMeters() * -1
              },
              ViewToWorldScale = zoomValue
            };
          }
          // it is a perspective view
          else
          {
            viewCenter = uidoc.ActiveView.Origin;
            //revit default value
            zoomValue = 45;

            ViewOrientation3D t = RevitUtils.ConvertBasePoint(doc, viewCenter, uidoc.ActiveView.ViewDirection,
             uidoc.ActiveView.UpDirection, false);

            XYZ c = t.EyePosition;
            XYZ vi = t.ForwardDirection;
            XYZ up = t.UpDirection;

            v.PerspectiveCamera = new PerspectiveCamera
            {
              CameraViewPoint =
              {
                X = c.X.ToMeters(),
                Y = c.Y.ToMeters(),
                Z = c.Z.ToMeters()
              },
              CameraUpVector =
              {
                X = up.X.ToMeters(),
                Y = up.Y.ToMeters(),
                Z = up.Z.ToMeters()
              },
              CameraDirection =
              {
                X = vi.X.ToMeters() * -1,
                Y = vi.Y.ToMeters() * -1,
                Z = vi.Z.ToMeters() * -1
              },
              FieldOfView = zoomValue
            };
          }

        }
        //COMPONENTS PART
        string versionName = doc.Application.VersionName;
        v.Components = new List<Component>();

        var visibleElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
          .WhereElementIsNotElementType()
          .WhereElementIsViewIndependent()
        .ToElementIds();
        var hiddenElems = new FilteredElementCollector(doc)
          .WhereElementIsNotElementType()
          .WhereElementIsViewIndependent()
          .Where(x => x.IsHidden(doc.ActiveView) 
            || !doc.ActiveView.IsElementVisibleInTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate, x.Id)).ToList();//would need to check how much this is affecting performance

        var selectedElems = uidoc.Selection.GetElementIds();


        //include only hidden elements and selected in the BCF
        if (visibleElems.Count() > hiddenElems.Count())
        {
        foreach (var elem in hiddenElems)
        {
          v.Components.Add(new Component
        {
          OriginatingSystem = versionName,
          IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, elem.Id)),
          Visible = false,
          Selected = false,
          AuthoringToolId = elem.Id.IntegerValue.ToString()
        });
        }
        foreach (var elem in selectedElems)
        {
          v.Components.Add(new Component
          {
            OriginatingSystem = versionName,
            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, elem)),
            Visible = true,
            Selected = true,
            AuthoringToolId = elem.IntegerValue.ToString()
          });
        }
        }
        //include only visigle elements
        //all the others are hidden
        else
        {
          foreach (var elem in visibleElems)
          {
            v.Components.Add(new Component
            {
              OriginatingSystem = versionName,
              IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, elem)),
              Visible = true,
              Selected = selectedElems.Contains(elem),
              AuthoringToolId = elem.IntegerValue.ToString()
            });
          }
        }
        return v;

      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error generating viewpoint", "exception: " + ex1);
      }
      return null;
    }

    /// <summary>
    /// passing event to the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
      e.Cancel = Bcfier.onClosing(e);
    }
    #endregion

    private void RevitWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      Task.Run(() =>
      {
        StatHat.Post.EzCounter(@"hello@teocomi.com", "BCFierRevitStart", 1);
      });
    }
  
  }
}