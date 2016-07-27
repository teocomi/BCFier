using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Navisworks.Api;
using Bcfier.Bcf.Bcf2;
using Bcfier.Data.Utils;

namespace Bcfier.Navisworks.Data
{
  public static class NavisView
  {
    public static void Open(Document doc, VisualizationInfo v)
    {
      try
      {

        NavisUtils.GetGunits(doc);
        Viewpoint viewpoint = new Viewpoint();

        //orthogonal
        if (v.OrthogonalCamera != null)
        {
          if (v.OrthogonalCamera.CameraViewPoint == null || v.OrthogonalCamera.CameraUpVector == null ||
              v.OrthogonalCamera.CameraDirection == null)
            return;

          var zoom = v.OrthogonalCamera.ViewToWorldScale.ToInternal();
          var cameraDirection = NavisUtils.GetNavisVector(v.OrthogonalCamera.CameraDirection);
          var cameraUpVector = NavisUtils.GetNavisVector(v.OrthogonalCamera.CameraUpVector);
          var cameraViewPoint = NavisUtils.GetNavisXYZ(v.OrthogonalCamera.CameraViewPoint);

          viewpoint.Position = cameraViewPoint;
          viewpoint.AlignUp(cameraUpVector);
          viewpoint.AlignDirection(cameraDirection);
          viewpoint.Projection = ViewpointProjection.Orthographic;
          viewpoint.FocalDistance = 1;

          //TODO
          //for better zooming from revit should use > zoom * 1.25
          //for better zooming from tekla should use > zoom / 1.25
          //still not sure why
          Point3D xyzTL = cameraViewPoint.Add(cameraUpVector.Multiply(zoom));
          var dist = xyzTL.DistanceTo(cameraViewPoint);
          viewpoint.SetExtentsAtFocalDistance(1, dist);
        }
        //perspective
        else if (v.PerspectiveCamera != null)
        {
          if (v.PerspectiveCamera.CameraViewPoint == null || v.PerspectiveCamera.CameraUpVector == null ||
              v.PerspectiveCamera.CameraDirection == null)
            return;

          var zoom = v.PerspectiveCamera.FieldOfView;
          var cameraDirection = NavisUtils.GetNavisVector(v.PerspectiveCamera.CameraDirection);
          var cameraUpVector = NavisUtils.GetNavisVector(v.PerspectiveCamera.CameraUpVector);
          var cameraViewPoint = NavisUtils.GetNavisXYZ(v.PerspectiveCamera.CameraViewPoint);

          viewpoint.Position = cameraViewPoint;
          viewpoint.AlignUp(cameraUpVector);
          viewpoint.AlignDirection(cameraDirection);
          viewpoint.Projection = ViewpointProjection.Perspective;
          viewpoint.FocalDistance = zoom;
        }

        doc.CurrentViewpoint.CopyFrom(viewpoint);


        //show/hide elements
        //todo: needs improvement
        //todo: add settings
        if (v.Components != null && v.Components.Any())
        {
          List<ModelItem> attachedElems = new List<ModelItem>();
          List<ModelItem> elems = doc.Models.First.RootItem.DescendantsAndSelf.ToList<ModelItem>();


          foreach (var item in elems.Where(o => o.InstanceGuid != Guid.Empty))
          {
            string ifcguid = IfcGuid.ToIfcGuid(item.InstanceGuid).ToString();
            if (v.Components.Any(o => o.IfcGuid == ifcguid))
              attachedElems.Add(item);

          }
          if (attachedElems.Any()) //avoid to hide everything if no elements matches
          {
            if (UserSettings.Get("selattachedelems") == "0")
            {
              List<ModelItem> elemsVisible = new List<ModelItem>();
              foreach (var item in attachedElems)
              {
                elemsVisible.AddRange(item.AncestorsAndSelf);
              }
              foreach (var item in elemsVisible)
                elems.Remove(item);

              doc.Models.ResetAllHidden();
              doc.Models.SetHidden(elems, true);
            }
            else
            {
              doc.CurrentSelection.Clear();
              doc.CurrentSelection.AddRange(attachedElems);
            }
          }
        }
      }

      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error opening view");
      }
    }

    //<summary>
    //Generate a VisualizationInfo of the current view
    //</summary>
    //<returns></returns>
    public static VisualizationInfo GenerateViewpoint(Document doc, Viewpoint viewpoint)
    {
      try
      {
        NavisUtils.GetGunits(doc);
        var v = new VisualizationInfo();

        Vector3D vi = GetViewDirection(viewpoint);
        Vector3D up = GetViewUp(viewpoint);
        Point3D c = new Point3D(viewpoint.Position.X, viewpoint.Position.Y, viewpoint.Position.Z);
        double zoomValue = 1;

        //prepare view
        viewpoint = viewpoint.CreateCopy();
        if (!viewpoint.HasFocalDistance)
          viewpoint.FocalDistance = 1;

        // it is a orthogonal view
        if (viewpoint.Projection == ViewpointProjection.Orthographic)
        {
          //TODO: needs checking!!!
          double dist = viewpoint.VerticalExtentAtFocalDistance / 2;
          zoomValue = 3.125 * dist / (up.Length * 1.25);


          v.OrthogonalCamera = new OrthogonalCamera
          {
            CameraViewPoint =
              {
                X = c.X.FromInternal(),
                Y = c.Y.FromInternal(),
                Z = c.Z.FromInternal()
              },
            CameraUpVector =
              {
                X = up.X.FromInternal(),
                Y = up.Y.FromInternal(),
                Z = up.Z.FromInternal()
              },
            CameraDirection =
              {
                X = vi.X.FromInternal(),
                Y = vi.Y.FromInternal(),
                Z = vi.Z.FromInternal()
              },
            ViewToWorldScale = zoomValue.FromInternal()
          };
        }
        else
        {
          zoomValue = viewpoint.FocalDistance;

          v.PerspectiveCamera = new PerspectiveCamera
          {
            CameraViewPoint =
              {
                X = c.X.FromInternal(),
                Y = c.Y.FromInternal(),
                Z = c.Z.FromInternal()
              },
            CameraUpVector =
              {
                X = up.X.FromInternal(),
                Y = up.Y.FromInternal(),
                Z = up.Z.FromInternal()
              },
            CameraDirection =
              {
                X = vi.X.FromInternal(),
                Y = vi.Y.FromInternal(),
                Z = vi.Z.FromInternal()
              },
            FieldOfView = zoomValue
          };

        }
        //COMPONENTS PART

        //if (elemCheck == 0)//visible (0)
        //  _elementList = _oDoc.Models.First.RootItem.DescendantsAndSelf.Where(o => o.InstanceGuid != Guid.Empty && ChechHidden(o.AncestorsAndSelf) && o.FindFirstGeometry() != null && !o.FindFirstGeometry().Item.IsHidden).ToList<ModelItem>();

        //if (null != _elementList && _elementList.Any() && elemCheck != 2)//not if none (2)
        //{
        //  v.Components = new Data.Component[_elementList.Count];
        //  string appname = Autodesk.Navisworks.Api.Application.Title;
        //  for (var i = 0; i < _elementList.Count; i++)
        //  {
        //    string ifcguid = IfcGuid.ToIfcGuid(_elementList.ElementAt(i).InstanceGuid).ToString();
        //    v.Components[i] = new Case.IssueTracker.Data.Component(appname, "", ifcguid);

        //  }
        //}
        return v;
      }

      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error generating viewpoint");
      }
      return null;

    }

    private static Vector3D GetViewDirection(Viewpoint v)
    {

      Rotation3D oRot = v.Rotation;
      // calculate view direction
      Rotation3D oNegtiveZ = new Rotation3D(0, 0, -1, 0);
      Rotation3D otempRot = MultiplyRotation3D(oNegtiveZ, oRot.Invert());
      Rotation3D oViewDirRot = MultiplyRotation3D(oRot, otempRot);
      // get view direction
      Vector3D oViewDir = new Vector3D(oViewDirRot.A, oViewDirRot.B, oViewDirRot.C);

      return oViewDir.Normalize();
    }
    private static Vector3D GetViewUp(Viewpoint v)
    {

      Rotation3D oRot = v.Rotation;
      // calculate view direction
      Rotation3D oNegtiveZ = new Rotation3D(0, 1, 0, 0);
      Rotation3D otempRot = MultiplyRotation3D(oNegtiveZ, oRot.Invert());
      Rotation3D oViewDirRot = MultiplyRotation3D(oRot, otempRot);
      // get view direction
      Vector3D oViewDir = new Vector3D(oViewDirRot.A, oViewDirRot.B, oViewDirRot.C);

      return oViewDir.Normalize();
    }
    //multiply two Rotation3D
    private static Rotation3D MultiplyRotation3D(Rotation3D r2,Rotation3D r1)
    {
      Rotation3D rot =new Rotation3D(
        r2.D * r1.A + r2.A * r1.D +
        r2.B * r1.C - r2.C * r1.B,
        r2.D * r1.B + r2.B * r1.D +
        r2.C * r1.A - r2.A * r1.C,
        r2.D * r1.C + r2.C * r1.D +
        r2.A * r1.B - r2.B * r1.A,
        r2.D * r1.D - r2.A * r1.A -
        r2.B * r1.B - r2.C * r1.C);
      rot.Normalize();
      return rot;
    }

  }
}
