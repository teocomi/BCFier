using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OpenProject.Shared.ViewModels.Bcf;
using OpenProject.Shared;

namespace OpenProject.Revit.Data
{
  //Methods for working with views
  public static class RevitView
  {
    //<summary>
    //Generate a VisualizationInfo of the current view
    //</summary>
    //<returns></returns>
    public static BcfViewpointViewModel GenerateViewpoint(UIDocument uidoc)
    {
      try
      {
        var doc = uidoc.Document;

        var v = new BcfViewpointViewModel();

        //Corners of the active UI view
        var topLeft = uidoc.GetOpenUIViews()[0].GetZoomCorners()[0];
        var bottomRight = uidoc.GetOpenUIViews()[0].GetZoomCorners()[1];

        //It's a 3d view
        if (uidoc.ActiveView.ViewType == ViewType.ThreeD)
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
            //length of the vector
            double dist = topLeft.DistanceTo(bottomRight) / 2;

            //ViewToWorldScale value
            zoomValue = dist * Math.Sin(diagVector.AngleTo(view3D.RightDirection)).ToMeters();

            // **** CUSTOM VALUE FOR TEKLA **** //
            // calculated experimentally, not sure why but it works
            //if (UserSettings.Get("optTekla") == "1")
            //  zoomValue = zoomValue * 2.5;
            // **** CUSTOM VALUE FOR TEKLA **** //

            ViewOrientation3D t = RevitUtils.ConvertBasePoint(doc, viewCenter, uidoc.ActiveView.ViewDirection,
            uidoc.ActiveView.UpDirection, false);

            XYZ c = t.EyePosition;
            XYZ vi = t.ForwardDirection;
            XYZ up = t.UpDirection;


            v.OrthogonalCamera = new BcfViewpointOrthogonalCameraViewModel
            {
              ViewPointX = c.X.ToMeters(),
              ViewPointY = c.Y.ToMeters(),
              ViewPointZ = c.Z.ToMeters(),
              UpX = up.X.ToMeters(),
              UpY = up.Y.ToMeters(),
              UpZ = up.Z.ToMeters(),
              DirectionX = vi.X.ToMeters() * -1,
              DirectionY = vi.Y.ToMeters() * -1,
              DirectionZ = vi.Z.ToMeters() * -1,
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

            v.PerspectiveCamera = new BcfViewpointPerspectiveCameraViewModel
            {
              ViewPointX = c.X.ToMeters(),
              ViewPointY = c.Y.ToMeters(),
              ViewPointZ = c.Z.ToMeters(),
              UpX = up.X.ToMeters(),
              UpY = up.Y.ToMeters(),
              UpZ = up.Z.ToMeters(),
              DirectionX = vi.X.ToMeters() * -1,
              DirectionY = vi.Y.ToMeters() * -1,
              DirectionZ = vi.Z.ToMeters() * -1,
              FieldOfView = zoomValue
            };
          }

        }
        //COMPONENTS PART
        string versionName = doc.Application.VersionName;

        var visibleElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
          .WhereElementIsNotElementType()
          .WhereElementIsViewIndependent()
        .ToElementIds();
        var hiddenElems = new FilteredElementCollector(doc)
          .WhereElementIsNotElementType()
          .WhereElementIsViewIndependent()
          .Where(x => x.IsHidden(doc.ActiveView)
            || !doc.ActiveView.IsElementVisibleInTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate, x.Id)).Select(x=>x.Id)
           ;//would need to check how much this is affecting performance

        var selectedElems = uidoc.Selection.GetElementIds();

        //TODO: set ViewSetupHints
        //TODO: create clipping planes
        //list of hidden components is smaller than the list of visible components
        foreach (var hiddenComponent in hiddenElems)
        {
          v.Components.Add(new BcfViewpointComponentViewModel
          {
            IsVisible = false,
            OriginatingSystem = versionName,
            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, hiddenComponent)),
            AuthoringToolId = hiddenComponent.IntegerValue.ToString()
          });
        }

        foreach (var visibleComponent in visibleElems)
        {
          v.Components.Add(new BcfViewpointComponentViewModel
          {
            IsVisible = true,
            OriginatingSystem = versionName,
            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, visibleComponent)),
            AuthoringToolId = visibleComponent.IntegerValue.ToString()
          });
        }

        foreach (var selectedComponent in selectedElems)
        {
          v.Components.Add(new BcfViewpointComponentViewModel
          {
            IsSelected = true,
            IsVisible = true,
            OriginatingSystem = versionName,
            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, selectedComponent)),
            AuthoringToolId = selectedComponent.IntegerValue.ToString()
          });
        }

        return v;

      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error generating viewpoint", "exception: " + ex1);
      }
      return null;
    }

  }
}
