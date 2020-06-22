namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfViewpointPerspectiveCameraViewModel : BcfViewpointCameraViewModelBase
  {
    private double _fieldOfView;

    public double FieldOfView
    {
      get => _fieldOfView;
      set => SetProperty(ref _fieldOfView, value);
    }
  }
}
