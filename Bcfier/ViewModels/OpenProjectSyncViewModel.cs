using Bcfier.Data;
using Bcfier.OpenProjectApi;
using Bcfier.OpenProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bcfier.ViewModels
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

    public OpenProjectSyncViewModel(string openProjectApiBaseUrl,
      string openProjectAccessToken,
      Action<string> openBcfFileByFileName)
    {
      _openProjectApiBaseUrl = openProjectApiBaseUrl;
      _openProjectAccessToken = openProjectAccessToken;
      _openBcfFileByFileName = openBcfFileByFileName;
      _openProjectClient = new OpenProjectClient(_openProjectAccessToken, _openProjectApiBaseUrl);
      SelectWorkPackageCommand = new RelayCommand(DownloadWorkPackageAsBcf);
      LoadProjects();
      this.PropertyChanged += (s, e) =>
        {
          if (e.PropertyName == nameof(SelectedProject))
          {
            WorkPackages.Clear();
            LoadWorkPackages();
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

    public ObservableCollection<WorkPackage> WorkPackages { get; } = new ObservableCollection<WorkPackage>();

    public ICommand SelectWorkPackageCommand { get; }

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
      var tempPath = Path.Combine(Path.GetTempPath(), "BCFier", Guid.NewGuid().ToString(), (SelectedWorkPackage.Subject ?? string.Empty) + ".bcf");
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

    public Project SelectedProject
    {
      get => _selectedProject;
      set
      {
        if (_selectedProject != value)
        {
          _selectedProject = value;
          NotifyPropertyChanged(nameof(SelectedProject));
        }
      }
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
      var pageSize = 200;
      var canLoadMore = true;
      while (canLoadMore)
      {
        canLoadMore = false;
        var workPackagesResult = await _openProjectClient.GetBcfWorkPackagesForProjectAsync(selectedProject.Id,
          pageSize,
          currentPage++);
        if (workPackagesResult.IsSuccess)
        {
          if (selectedProject == SelectedProject)
          {
            // This guards against stale work packages, e.g. a request for work packages
            // that finishes after the user has changed the selected project
            foreach (var newWorkPackage in workPackagesResult.Result)
            {
              WorkPackages.Add(newWorkPackage);
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
