using Dangl;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfViewpointClippingPlaneViewModel : BindableBase
  {
    private BcfPointOrVectorViewModel _direction;
    private BcfPointOrVectorViewModel _location;

    public BcfPointOrVectorViewModel Direction
    {
      get => _direction;
      set => SetProperty(ref _direction, value);
    }

    public BcfPointOrVectorViewModel Location
    {
      get => _location;
      set => SetProperty(ref _location, value);
    }
  }
}
