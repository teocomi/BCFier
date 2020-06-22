using Dangl;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfPointOrVectorViewModel : BindableBase
  {
    private double _x;
    private double _y;
    private double _z;

    public double X
    {
      get => _x;
      set => SetProperty(ref _x, value);
    }

    public double Y
    {
      get => _y;
      set => SetProperty(ref _y, value);
    }

    public double Z
    {
      get => _z;
      set => SetProperty(ref _z, value);
    }
  }
}
