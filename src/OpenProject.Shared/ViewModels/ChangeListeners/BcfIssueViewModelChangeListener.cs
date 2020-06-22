using OpenProject.Shared.ViewModels.Bcf;
using System;
using System.ComponentModel;

namespace OpenProject.Shared.ViewModels.ChangeListeners
{
  internal class BcfIssueViewModelChangeListener
  {
    private readonly BcfIssueViewModel _bcfIssue;

    public BcfIssueViewModelChangeListener(BcfIssueViewModel bcfIssue)
    {
      bcfIssue.PropertyChanged += BcfIssue_PropertyChanged;
      _bcfIssue = bcfIssue;

      _collectionChangeListener = new CollectionChangeListener(_bcfIssue.Viewpoints);
      _collectionChangeListener.PropertyChanged += (s, e) =>
      {
        if (!DisableListening)
        {
          UpdateModifiedDate();
        }
      };
    }

    public bool DisableListening { get; set; }

    private ChangeListener _markupChangeListener;
    private CollectionChangeListener _collectionChangeListener;

    private void UpdateModifiedDate()
    {
      var disableListeningWasActivated = DisableListening;
      DisableListening = true;

      _bcfIssue.IsModified = true;
      if (_bcfIssue.Markup == null)
      {
        _bcfIssue.Markup = new BcfMarkupViewModel();
      }
      if (_bcfIssue.Markup.BcfTopic == null)
      {
        _bcfIssue.Markup.BcfTopic = new BcfTopicViewModel();
      }

      _bcfIssue.Markup.BcfTopic.ModifiedDate = DateTime.UtcNow;
      _bcfIssue.IsModified = true;

      if (!disableListeningWasActivated)
      {
        DisableListening = false;
      }
    }

    private void BcfIssue_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!DisableListening)
      {
        UpdateModifiedDate();
      }

      if (e.PropertyName == nameof(BcfIssueViewModel.Markup))
      {
        if (_markupChangeListener != null)
        {
          _markupChangeListener.PropertyChanged -= MarkupChangeListener_PropertyChanged;
        }
        _markupChangeListener = new ChangeListener(_bcfIssue.Markup);
        _markupChangeListener.PropertyChanged += MarkupChangeListener_PropertyChanged;
      }
    }

    private void MarkupChangeListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!DisableListening)
      {
        UpdateModifiedDate();
      }
    }
  }
}
