using Dangl;
using OpenProject.Shared.ViewModels.Bcf;
using System.Collections.ObjectModel;

namespace OpenProject.ViewModels
{
  public class PanelViewModel : BindableBase
  {
    private BcfFileViewModel _selectedBcfFile;

    public BcfFileViewModel SelectedBcfFile
    {
      get => _selectedBcfFile;
      set => SetProperty(ref _selectedBcfFile, value);
    }

    public string BcfierVersion => "BCFier " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

    public ObservableCollection<BcfFileViewModel> BcfFiles { get; } = new ObservableCollection<BcfFileViewModel>();
  }
}
