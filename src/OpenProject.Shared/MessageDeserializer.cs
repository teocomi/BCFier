using iabi.BCF.APIObjects.V21;
using iabi.BCF.Converter;
using Newtonsoft.Json.Linq;
using OpenProject.Shared.ViewModels.Bcf;
using System;
using System.Linq;

namespace OpenProject.Shared
{
  public static class MessageDeserializer
  {
    public static BcfViewpointViewModel DeserializeBcfViewpoint(WebUIMessageEventArgs webUIMessage)
    {
      if (webUIMessage.MessageType != MessageTypes.VIEWPOINT_DATA)
      {
        throw new InvalidOperationException("Tried to deserialize a message with the wrong data type");
      }

      var deserializedJson = JObject.Parse(webUIMessage.MessagePayload.Trim('"').Replace("\\\"", "\""));

      var bcfApiViewpoint = new ViewpointApiMessage
      {
        Viewpoint = deserializedJson.ToObject<Viewpoint_GET>(),
        Components = deserializedJson.ToObject<Components>()
      };

      var bcfViewpoint = new BcfViewpointViewModel
      {
        Id = bcfApiViewpoint.Viewpoint.Guid,
      };

      if (bcfApiViewpoint.Components != null)
      {
        var defaultVisibility = bcfApiViewpoint.Components.Visibility?.Default_visibility ?? true;
        if (bcfApiViewpoint.Components.Coloring?.Any() ?? false)
        {
          foreach (var bcfApiComponentColor in bcfApiViewpoint.Components.Coloring)
          {
            foreach (var bcfApiComponent in bcfApiComponentColor.Components)
            {
              bcfViewpoint.Components.Add(new BcfViewpointComponentViewModel
              {
                AuthoringToolId = bcfApiComponent.Authoring_tool_id,
                Color = bcfApiComponentColor.Color.ToByteArrayFromHexRgbColor(),
                IfcGuid = bcfApiComponent.Ifc_guid,
                IsSelected = false,
                IsVisible = defaultVisibility,
                OriginatingSystem = bcfApiComponent.Originating_system
              });
            }
          }
        }

        if (bcfApiViewpoint.Components.Selection?.Any() ?? false)
        {
          foreach (var bcfApiComponent in bcfApiViewpoint.Components.Selection)
          {
            bcfViewpoint.Components.Add(new BcfViewpointComponentViewModel
            {
              AuthoringToolId = bcfApiComponent.Authoring_tool_id,
              IfcGuid = bcfApiComponent.Ifc_guid,
              IsSelected = true,
              IsVisible = defaultVisibility,
              OriginatingSystem = bcfApiComponent.Originating_system
            });
          }
        }

        if (bcfApiViewpoint.Components.Visibility != null)
        {
          foreach (var bcfApiComponent in bcfApiViewpoint.Components.Visibility.Exceptions)
          {
            bcfViewpoint.Components.Add(new BcfViewpointComponentViewModel
            {
              AuthoringToolId = bcfApiComponent.Authoring_tool_id,
              IfcGuid = bcfApiComponent.Ifc_guid,
              IsSelected = false,
              IsVisible = !defaultVisibility,
              OriginatingSystem = bcfApiComponent.Originating_system
            });
          }
        }
      }

      if (bcfApiViewpoint.Viewpoint?.Clipping_planes?.Any() ?? false)
      {
        foreach (var bcfApiClippingPlane in bcfApiViewpoint.Viewpoint.Clipping_planes)
        {
          bcfViewpoint.ClippingPlanes.Add(new BcfViewpointClippingPlaneViewModel
          {
            Direction = new BcfPointOrVectorViewModel
            {
              X = bcfApiClippingPlane.Direction.X,
              Y = bcfApiClippingPlane.Direction.Y,
              Z = bcfApiClippingPlane.Direction.Z
            },
            Location = new BcfPointOrVectorViewModel
            {
              X = bcfApiClippingPlane.Location.X,
              Y = bcfApiClippingPlane.Location.Y,
              Z = bcfApiClippingPlane.Location.Z
            }
          });
        }
      }

      if (bcfApiViewpoint.Viewpoint?.Lines?.Any() ?? false)
      {
        foreach (var bcfApiLine in bcfApiViewpoint.Viewpoint.Lines)
        {
          bcfViewpoint.Lines.Add(new BcfViewpointLineViewModel
          {
            Start = new BcfPointOrVectorViewModel
            {
              X = bcfApiLine.Start_point.X,
              Y = bcfApiLine.Start_point.Y,
              Z = bcfApiLine.Start_point.Z
            },
            End = new BcfPointOrVectorViewModel
            {
              X = bcfApiLine.End_point.X,
              Y = bcfApiLine.End_point.Y,
              Z = bcfApiLine.End_point.Z
            }
          });
        }
      }

      if (bcfApiViewpoint.Viewpoint?.Orthogonal_camera != null)
      {
        bcfViewpoint.OrthogonalCamera = new BcfViewpointOrthogonalCameraViewModel
        {
          DirectionX = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_direction.X,
          DirectionY = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_direction.Y,
          DirectionZ = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_direction.Z,
          UpX = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_up_vector.X,
          UpY = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_up_vector.Y,
          UpZ = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_up_vector.Z,
          ViewPointX = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_view_point.X,
          ViewPointY = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_view_point.Y,
          ViewPointZ = bcfApiViewpoint.Viewpoint.Orthogonal_camera.Camera_view_point.Z,
          ViewToWorldScale = bcfApiViewpoint.Viewpoint.Orthogonal_camera.View_to_world_scale
        };
      }
      else if (bcfApiViewpoint.Viewpoint?.Perspective_camera != null)
      {
        bcfViewpoint.PerspectiveCamera = new BcfViewpointPerspectiveCameraViewModel
        {
          DirectionX = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_direction.X,
          DirectionY = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_direction.Y,
          DirectionZ = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_direction.Z,
          UpX = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_up_vector.X,
          UpY = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_up_vector.Y,
          UpZ = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_up_vector.Z,
          ViewPointX = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_view_point.X,
          ViewPointY = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_view_point.Y,
          ViewPointZ = bcfApiViewpoint.Viewpoint.Perspective_camera.Camera_view_point.Z,
          FieldOfView = bcfApiViewpoint.Viewpoint.Perspective_camera.Field_of_view
        };
      }
      return bcfViewpoint;
    }
  }
}
