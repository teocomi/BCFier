using Dangl;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfViewpointComponentViewModel : BindableBase
  {
    private string _authoringToolId;
    private byte[] _color;
    private string _ifcGuid;
    private string _originatingSystem;
    private bool _isSelected;
    private bool _isVisible;

    public string AuthoringToolId
    {
      get => _authoringToolId;
      set => SetProperty(ref _authoringToolId, value);
    }

    public byte[] Color
    {
      get => _color;
      set => SetProperty(ref _color, value);
    }

    public string IfcGuid
    {
      get => _ifcGuid;
      set => SetProperty(ref _ifcGuid, value);
    }

    public string OriginatingSystem
    {
      get => _originatingSystem;
      set => SetProperty(ref _originatingSystem, value);
    }

    public bool IsSelected
    {
      get => _isSelected;
      set => SetProperty(ref _isSelected, value);
    }

    public bool IsVisible
    {
      get => _isVisible;
      set => SetProperty(ref _isVisible, value);
    }
  }
}
