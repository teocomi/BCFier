using Dangl;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public abstract class BcfViewpointCameraViewModelBase : BindableBase
  {
    private double _directionX;
    private double _directionY;
    private double _directionZ;
    private double _upX;
    private double _upY;
    private double _upZ;
    private double _viewPointX;
    private double _viewPointY;
    private double _viewPointZ;

    public double DirectionX
    {
      get => _directionX;
      set => SetProperty(ref _directionX, value);
    }

    public double DirectionY
    {
      get => _directionY;
      set => SetProperty(ref _directionY, value);
    }

    public double DirectionZ
    {
      get => _directionZ;
      set => SetProperty(ref _directionZ, value);
    }

    public double UpX
    {
      get => _upX;
      set => SetProperty(ref _upX, value);
    }

    public double UpY
    {
      get => _upY;
      set => SetProperty(ref _upY, value);
    }

    public double UpZ
    {
      get => _upZ;
      set => SetProperty(ref _upZ, value);
    }

    public double ViewPointX
    {
      get => _viewPointX;
      set => SetProperty(ref _viewPointX, value);
    }

    public double ViewPointY
    {
      get => _viewPointY;
      set => SetProperty(ref _viewPointY, value);
    }

    public double ViewPointZ
    {
      get => _viewPointZ;
      set => SetProperty(ref _viewPointZ, value);
    }
  }
}
