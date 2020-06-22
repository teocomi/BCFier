using Dangl;
using System;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfMarkupViewpointReferenceViewModel : BindableBase
  {
    private Guid _id = Guid.NewGuid();
    private byte[] _snapshot;

    public Guid Id
    {
      get => _id;
      set => SetProperty(ref _id, value);
    }

    public byte[] Snapshot
    {
      get => _snapshot;
      set => SetProperty(ref _snapshot, value);
    }
  }
}
