using Bcfier.ViewModels.Bcf;
using Dangl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bcfier.ViewModels
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
