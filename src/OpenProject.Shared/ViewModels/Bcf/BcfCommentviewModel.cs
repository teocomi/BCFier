using Dangl;
using System;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfCommentviewModel : BindableBase
  {
    private string _author;
    private string _modifiedAuthor;
    private DateTime _creationDate = DateTime.UtcNow;
    private DateTime? _modifiedDate;
    private string _text;
    private Guid _id = Guid.NewGuid();
    private string _status;
    private Guid? _viewpointId;

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

    public string Text
    {
      get => _text;
      set => SetProperty(ref _text, value);
    }

    public Guid Id
    {
      get => _id;
      set => SetProperty(ref _id, value);
    }

    public string Status
    {
      get => _status;
      set => SetProperty(ref _status, value);
    }

    public Guid? ViewpointId
    {
      get => _viewpointId;
      set => SetProperty(ref _viewpointId, value);
    }
  }
}
