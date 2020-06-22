using Dangl;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfFileViewModel : BindableBase
  {
    public BcfFileViewModel()
    {
      BcfIssues.CollectionChanged += (s, e) =>
      {
        FilterTopics();

        if (e.NewItems?.Count > 0)
        {
          foreach (var bcfIssue in e.NewItems.OfType<BcfIssueViewModel>())
          {
            bcfIssue.PropertyChanged += BcfIssue_PropertyChanged;
          }
        }
        if (e.OldItems?.Count > 0)
        {
          foreach (var bcfIssue in e.OldItems.OfType<BcfIssueViewModel>())
          {
            bcfIssue.PropertyChanged -= BcfIssue_PropertyChanged;
          }
        }
      };
    }

    private void BcfIssue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(BcfIssueViewModel.IsModified)
        && (sender as BcfIssueViewModel).IsModified)
      {
        IsModified = true;
      }
    }

    private string _fileName;
    private string _fullName;
    private bool _isModified;
    private Guid _id = Guid.NewGuid();
    private BcfVersion _bcfVersion;
    private string _projectId;
    private string _projectName;
    private BcfIssueViewModel _selectedBcfIssue;
    private string _textSearch;

    public string FileName
    {
      get => _fileName;
      set => SetProperty(ref _fileName, value);
    }

    public string FullName
    {
      get => _fullName;
      set => SetProperty(ref _fullName, value);
    }

    public bool IsModified
    {
      get => _isModified;
      set => SetProperty(ref _isModified, value);
    }

    public Guid Id
    {
      get => _id;
      set => SetProperty(ref _id, value);
    }

    public BcfVersion BcfVersion
    {
      get => _bcfVersion;
      set => SetProperty(ref _bcfVersion, value);
    }

    public ObservableCollection<BcfIssueViewModel> BcfIssues { get; } = new ObservableCollection<BcfIssueViewModel>();

    public ObservableCollection<BcfIssueViewModel> BcfIssuesFiltered { get; } = new ObservableCollection<BcfIssueViewModel>();

    public string ProjectId
    {
      get => _projectId;
      set => SetProperty(ref _projectId, value);
    }

    public string ProjectName
    {
      get => _projectName;
      set => SetProperty(ref _projectName, value);
    }

    public BcfIssueViewModel SelectedBcfIssue
    {
      get => _selectedBcfIssue;
      set => SetProperty(ref _selectedBcfIssue, value);
    }

    public string TextSearch
    {
      get => _textSearch;
      set
      {
        if (SetProperty(ref _textSearch, value))
        {
          FilterTopics();
        }
      }
    }

    private void FilterTopics()
    {
      BcfIssuesFiltered.Clear();

      var searchString = TextSearch?.ToLowerInvariant();
      if (string.IsNullOrWhiteSpace(searchString))
      {
        foreach (var issue in BcfIssues)
        {
          BcfIssuesFiltered.Add(issue);
        }
        return;
      }

      foreach (var issue in BcfIssues)
      {
        var shouldShowIssue = (issue.Markup?.BcfTopic?.Title?.ToLowerInvariant().Contains(searchString) ?? false)
          || (issue.Markup?.BcfTopic?.Description?.ToLowerInvariant().Contains(searchString) ?? false)
          || (issue.Markup?.Comments?.Any(c => c.Text?.ToLowerInvariant()?.Contains(searchString) ?? false) ?? false);

        if (shouldShowIssue)
        {
          BcfIssuesFiltered.Add(issue);
        }
      }
    }
  }
}
