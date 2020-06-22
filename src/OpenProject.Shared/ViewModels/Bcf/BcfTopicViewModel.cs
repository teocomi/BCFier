using Dangl;
using System;
using System.Collections.ObjectModel;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfTopicViewModel : BindableBase
  {
    private Guid _id = Guid.NewGuid();
    private string _author;
    private string _modifiedAuthor;
    private DateTime _creationDate = DateTime.UtcNow;
    private DateTime? _modifiedDate;
    private string _assignedTo;
    private string _description;
    private string _title;
    private string _status;
    private string _type;
    private string _priority;

    public Guid Id
    {
      get => _id;
      set => SetProperty(ref _id, value);
    }

    public string Author
    {
      get => _author;
      set => SetProperty(ref _author, value);
    }

    public string ModifiedAuthor
    {
      get => _modifiedAuthor;
      set => SetProperty(ref _modifiedAuthor, value);
    }

    public DateTime CreationDate
    {
      get => _creationDate;
      set => SetProperty(ref _creationDate, value);
    }

    public DateTime? ModifiedDate
    {
      get => _modifiedDate;
      set => SetProperty(ref _modifiedDate, value);
    }

    public string AssignedTo
    {
      get => _assignedTo;
      set => SetProperty(ref _assignedTo, value);
    }

    public string Description
    {
      get => _description;
      set => SetProperty(ref _description, value);
    }

    public string Title
    {
      get => _title;
      set => SetProperty(ref _title, value);
    }

    public string Status
    {
      get => _status;
      set => SetProperty(ref _status, value);
    }

    public string Type
    {
      get => _type;
      set => SetProperty(ref _type, value);
    }

    public string Priority
    {
      get => _priority;
      set => SetProperty(ref _priority, value);
    }

    public ObservableCollection<string> Labels { get; } = new ObservableCollection<string>();

    public ObservableCollection<string> AvailableStati { get; } = new ObservableCollection<string>();
    public ObservableCollection<string> AvailableTypes { get; } = new ObservableCollection<string>();
  }
}
