using Dangl;
using System.Collections.ObjectModel;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfMarkupViewModel : BindableBase
  {
    private BcfTopicViewModel _bcfTopic;

    public BcfTopicViewModel BcfTopic
    {
      get => _bcfTopic;
      set => SetProperty(ref _bcfTopic, value);
    }

    public ObservableCollection<BcfCommentviewModel> Comments { get; } = new ObservableCollection<BcfCommentviewModel>();

    public ObservableCollection<BcfHeaderFileViewModel> HeaderFiles { get; } = new ObservableCollection<BcfHeaderFileViewModel>();
    public ObservableCollection<BcfMarkupViewpointReferenceViewModel> ViewpointReferences { get; } = new ObservableCollection<BcfMarkupViewpointReferenceViewModel>();
  }
}
