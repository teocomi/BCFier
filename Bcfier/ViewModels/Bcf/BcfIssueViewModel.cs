using System.Collections.ObjectModel;
using Bcfier.ViewModels.ChangeListeners;
using Dangl;

namespace Bcfier.ViewModels.Bcf
{
  public class BcfIssueViewModel : BindableBase
  {
    private BcfMarkupViewModel _markup;
    private bool _isModified;
    private BcfIssueViewModelChangeListener _changeListener;

    public BcfIssueViewModel()
    {
      _changeListener = new BcfIssueViewModelChangeListener(this);
    }

    public bool DisableListeningForChanges
    {
      get => _changeListener.DisableListening;
      set => _changeListener.DisableListening = value;
    }

    public BcfMarkupViewModel Markup
    {
      get => _markup;
      set => SetProperty(ref _markup, value);
    }

    public bool IsModified
    {
      get => _isModified;
      internal set => SetProperty(ref _isModified, value);
    }

    public ObservableCollection<BcfViewpointViewModel> Viewpoints { get; } = new ObservableCollection<BcfViewpointViewModel>();
  }
}
