using OpenProject.Data;
using OpenProject.OpenProjectApi;
using OpenProject.OpenProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace OpenProject.ViewModels
{
  public class OpenProjectSyncViewModel : INotifyPropertyChanged
  {
    private readonly string _openProjectApiBaseUrl;
    private readonly string _openProjectAccessToken;
    private readonly Action<string> _openBcfFileByFileName;
    private readonly OpenProjectClient _openProjectClient;
    private bool _projectsLoaded = false;
    private List<Project> _projects;
    private Project _selectedProject;
    private WorkPackage _selectedWorkPackage;
    private string _issueFilter;
    private readonly Timer _filterRefreshTimer = new Timer(500);

    public OpenProjectSyncViewModel(string openProjectApiBaseUrl,
      string openProjectAccessToken,
      Action<string> openBcfFileByFileName)
    {
      _openProjectApiBaseUrl = openProjectApiBaseUrl;
      _openProjectAccessToken = openProjectAccessToken;
      _openBcfFileByFileName = openBcfFileByFileName;
      _openProjectClient = new OpenProjectClient(_openProjectAccessToken, _openProjectApiBaseUrl);
      SelectWorkPackageCommand = new RelayCommand(DownloadWorkPackageAsBcf);
      OpenAllWorkPackagesInProjectCommand = new RelayCommand(DownloadAllWorkPackagesAsBcf);
      LoadProjects();

      var shouldRequeryDueToTextFilterChange = false;
      _filterRefreshTimer.Elapsed += (s, e) =>
        {
          if (shouldRequeryDueToTextFilterChange)
          {
            try
            {
              Application.Current.Dispatcher.Invoke(() =>
              {
                WorkPackages.Clear();
              });
            }
            catch
            {
              // We might not have Application.Current in Revit
              WorkPackages.Clear();
            }
            LoadWorkPackages();
            shouldRequeryDueToTextFilterChange = false;
          }
        };
      _filterRefreshTimer.Start();

      PropertyChanged += (s, e) =>
        {
          if (e.PropertyName == nameof(SelectedProject))
          {
            WorkPackages.Clear();
            LoadWorkPackages();
          }
          else if (e.PropertyName == nameof(IssueFilter))
          {
            shouldRequeryDueToTextFilterChange = true;
          }
        };
    }

    public bool ProjectsLoaded
    {
      get => _projectsLoaded;
      set
      {
        if (_projectsLoaded != value)
        {
          _projectsLoaded = value;
          NotifyPropertyChanged(nameof(ProjectsLoaded));
        }
      }
    }


    public List<Project> Projects
    {
      get => _projects;
      set
      {
        if (_projects != value)
        {
          _projects = value;
          NotifyPropertyChanged(nameof(Projects));
        }
      }
    }

    public Project SelectedProject
    {
      get => _selectedProject;
      set
      {
        if (_selectedProject != value)
        {
          _selectedProject = value;
          NotifyPropertyChanged(nameof(SelectedProject));
          NotifyPropertyChanged(nameof(ProjectIsSelected));
        }
      }
    }

    public bool ProjectIsSelected => SelectedProject != null;

    public WorkPackage SelectedWorkPackage
    {
      get => _selectedWorkPackage;
      set
      {
        if (_selectedWorkPackage != value)
        {
          _selectedWorkPackage = value;
          NotifyPropertyChanged(nameof(SelectedWorkPackage));
        }
      }
    }

    public string IssueFilter
    {
      get => _issueFilter;
      set
      {
        if (_issueFilter != value)
        {
          _issueFilter = value;
          NotifyPropertyChanged(nameof(IssueFilter));
        }
      }
    }

    public ObservableCollection<WorkPackage> WorkPackages { get; } = new ObservableCollection<WorkPackage>();

    public ICommand SelectWorkPackageCommand { get; }
    public ICommand OpenAllWorkPackagesInProjectCommand { get; }

    private void DownloadWorkPackageAsBcf()
    {
      DownloadWorkPackageAsBcfAsync();
    }

    private async Task DownloadWorkPackageAsBcfAsync()
    {
      var wpDownload = await _openProjectClient
        .GetBcfWorkPackageAsBcfXmlAsync(SelectedProject.Id, SelectedWorkPackage.Id);

      // The '.bcf' file extension indicates that this is a BCF XML v2.1, the format that's currently exported by Open Project.
      // This is relevant later down since we're using the original methods to load a BCF XML file via a temp file
      var tempPath = Path.Combine(Path.GetTempPath(), "BCFier", Guid.NewGuid().ToString() + ".bcf");
      if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
      }
      using (var fileStream = File.Create(tempPath))
      {
        await wpDownload.Response.Content.CopyToAsync(fileStream);
      }

      _openBcfFileByFileName(tempPath);

      OnCloseWindowRequested?.Invoke(this, new EventArgs());
    }

    private void DownloadAllWorkPackagesAsBcf()
    {
      DownloadAllWorkPackagesAsBcfAsync();
    }

    private async Task DownloadAllWorkPackagesAsBcfAsync()
    {
      var wpDownload = await _openProjectClient
        .DownloadAllBcfWorkPackagesInProject(SelectedProject.Id);

      // The '.bcf' file extension indicates that this is a BCF XML v2.1, the format that's currently exported by Open Project.
      // This is relevant later down since we're using the original methods to load a BCF XML file via a temp file
      var tempPath = Path.Combine(Path.GetTempPath(), "BCFier", Guid.NewGuid().ToString(), (SelectedProject?.Name ?? string.Empty) + ".bcf");
      if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
      }
      using (var fileStream = File.Create(tempPath))
      {
        await wpDownload.Response.Content.CopyToAsync(fileStream);
      }

      _openBcfFileByFileName(tempPath);

      OnCloseWindowRequested?.Invoke(this, new EventArgs());
    }

    private async Task LoadProjects()
    {
      var projectsResponse = await _openProjectClient.GetAllProjectsAsync();
      if (projectsResponse.IsSuccess)
      {
        ProjectsLoaded = true;
        Projects = projectsResponse.Result;
        if (Projects.Any())
        {
          SelectedProject = Projects.First();
        }
      }
    }

    private async Task LoadWorkPackages()
    {
      var selectedProject = SelectedProject;
      if (selectedProject == null)
      {
        return;
      }

      var currentPage = 1;
      var pageSize = 500;
      var canLoadMore = true;
      var userTextQuery = IssueFilter;
      while (canLoadMore)
      {
        canLoadMore = false;
        var workPackagesResult = await _openProjectClient.GetBcfWorkPackagesForProjectAsync(selectedProject.Id,
          pageSize,
          currentPage++,
          userTextQuery);
        if (workPackagesResult.IsSuccess)
        {
          if (selectedProject == SelectedProject && userTextQuery == IssueFilter)
          {
            // This guards against stale work packages, e.g. a request for work packages
            // that finishes after the user has changed the selected project
            // Guards also against changed IssueFilters
            foreach (var newWorkPackage in workPackagesResult.Result)
            {
              try
              {
                Application.Current.Dispatcher.Invoke(() =>
                {
                  WorkPackages.Add(newWorkPackage);
                });
              }
              catch
              {
                // We might not have Application.Current in Revit
                WorkPackages.Add(newWorkPackage);
              }
            }
            NotifyPropertyChanged(nameof(WorkPackages));
            if (workPackagesResult.Result.Count >= pageSize)
            {
              // We're just assuming that if the page is full, there might
              // be another one available to grab
              canLoadMore = true;
            }
          }
        }
      }
    }

    public event EventHandler OnCloseWindowRequested;

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
