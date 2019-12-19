using Bcfier.ViewModels.Bcf;
using System;
using System.Linq;

namespace Bcfier.WebViewIntegration
{
  public static class MessageSerializer
  {
    public static ViewpointApiMessage SerializeBcfViewpoint(BcfViewpointViewModel bcfViewpointViewModel)
    {
      var apiViewpoint = new ViewpointApiMessage
      {
        Viewpoint = new iabi.BCF.APIObjects.V21.Viewpoint_GET
        {
          Clipping_planes = bcfViewpointViewModel.ClippingPlanes?.Select(cp => new iabi.BCF.APIObjects.V21.Clipping_plane
          {
            Direction = new iabi.BCF.APIObjects.V21.Direction
            {
              X = Convert.ToSingle(cp.Direction.X),
              Y = Convert.ToSingle(cp.Direction.Y),
              Z = Convert.ToSingle(cp.Direction.Z)
            },
            Location = new iabi.BCF.APIObjects.V21.Location
            {
              X = Convert.ToSingle(cp.Location.X),
              Y = Convert.ToSingle(cp.Location.Y),
              Z = Convert.ToSingle(cp.Location.Z)
            }
          }).ToList(),
          Lines = bcfViewpointViewModel.Lines?.Select(l => new iabi.BCF.APIObjects.V21.Line
          {
            Start_point = new iabi.BCF.APIObjects.V21.Point
            {
              X = Convert.ToSingle(l.Start.X),
              Y = Convert.ToSingle(l.Start.Y),
              Z = Convert.ToSingle(l.Start.Z)
            },
            End_point = new iabi.BCF.APIObjects.V21.Point
            {
              X = Convert.ToSingle(l.End.X),
              Y = Convert.ToSingle(l.End.Y),
              Z = Convert.ToSingle(l.End.Z)
            }
          }).ToList(),
          Orthogonal_camera = bcfViewpointViewModel.OrthogonalCamera == null ? null : new iabi.BCF.APIObjects.V21.Orthogonal_camera
          {
            View_to_world_scale = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.ViewToWorldScale),
            Camera_direction = new iabi.BCF.APIObjects.V21.Direction
            {
              X = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.DirectionX),
              Y = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.DirectionY),
              Z = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.DirectionZ)
            },
            Camera_up_vector = new iabi.BCF.APIObjects.V21.Direction
            {
              X = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.UpX),
              Y = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.UpY),
              Z = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.UpZ)
            },
            Camera_view_point = new iabi.BCF.APIObjects.V21.Point
            {
              X = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.ViewPointX),
              Y = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.ViewPointY),
              Z = Convert.ToSingle(bcfViewpointViewModel.OrthogonalCamera.ViewPointZ)
            }
          },
          Perspective_camera = bcfViewpointViewModel.PerspectiveCamera == null ? null : new iabi.BCF.APIObjects.V21.Perspective_camera
          {
            Field_of_view = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.FieldOfView),
            Camera_direction = new iabi.BCF.APIObjects.V21.Direction
            {
              X = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.DirectionX),
              Y = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.DirectionY),
              Z = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.DirectionZ)
            },
            Camera_up_vector = new iabi.BCF.APIObjects.V21.Direction
            {
              X = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.UpX),
              Y = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.UpY),
              Z = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.UpZ)
            },
            Camera_view_point = new iabi.BCF.APIObjects.V21.Point
            {
              X = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.ViewPointX),
              Y = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.ViewPointY),
              Z = Convert.ToSingle(bcfViewpointViewModel.PerspectiveCamera.ViewPointZ)
            }
          },
          Guid = bcfViewpointViewModel.Id
        },
      };

      if (bcfViewpointViewModel.Components?.Any() ?? false)
      {
        apiViewpoint.Components = new iabi.BCF.APIObjects.V21.Components();
        apiViewpoint.Components.Coloring = new System.Collections.Generic.List<iabi.BCF.APIObjects.V21.Coloring>();
        apiViewpoint.Components.Selection = new System.Collections.Generic.List<iabi.BCF.APIObjects.V21.Component>();
        apiViewpoint.Components.Visibility = new iabi.BCF.APIObjects.V21.Visibility();

        foreach (var component in bcfViewpointViewModel.Components)
        {
          if (component.Color != null)
          {

          }
          else if (component.IsSelected)
          {

          }
          else
          {

          }
        }
      }

      return apiViewpoint;
    }
  }
}
