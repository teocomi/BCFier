namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfViewpointOrthogonalCameraViewModel : BcfViewpointCameraViewModelBase
  {
    private double _viewToWorldScale;

    public double ViewToWorldScale
    {
      get => _viewToWorldScale;
      set => SetProperty(ref _viewToWorldScale, value);
    }
  }
}
