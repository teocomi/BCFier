using OpenProject.Shared.ViewModels.Bcf;

namespace OpenProject.Revit.Extensions
{
  public static class BcfViewpointViewModelExtensions
  {
    public static void EnsurePerspectiveCameraVectorsAreOrthogonal(this BcfViewpointViewModel bcfViewpointViewModel)
    {
      var perspectiveCamera = bcfViewpointViewModel?.PerspectiveCamera;
      if (perspectiveCamera == null)
      {
        return;
      }

      if (perspectiveCamera.DirectionX != 0)
      {
        perspectiveCamera.UpX = -1 * (perspectiveCamera.DirectionY * perspectiveCamera.UpY + perspectiveCamera.DirectionZ * perspectiveCamera.UpZ) / perspectiveCamera.DirectionX;
      }
      else if (perspectiveCamera.DirectionY != 0)
      {
        perspectiveCamera.UpY = -1 * (perspectiveCamera.DirectionX * perspectiveCamera.UpX + perspectiveCamera.DirectionZ * perspectiveCamera.UpZ) / perspectiveCamera.DirectionY;
      }
      else if (perspectiveCamera.DirectionZ != 0)
      {
        perspectiveCamera.UpZ = -1 * (perspectiveCamera.DirectionX * perspectiveCamera.UpX + perspectiveCamera.DirectionY * perspectiveCamera.UpY) / perspectiveCamera.DirectionZ;
      }
    }
  }
}
