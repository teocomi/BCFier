using Dangl;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfViewpointLineViewModel : BindableBase
  {
    private BcfPointOrVectorViewModel _start;
    private BcfPointOrVectorViewModel _end;

    public BcfPointOrVectorViewModel Start
    {
      get => _start;
      set => SetProperty(ref _start, value);
    }

    public BcfPointOrVectorViewModel End
    {
      get => _end;
      set => SetProperty(ref _end, value);
    }
  }
}
