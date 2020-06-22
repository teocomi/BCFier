using Dangl;
using System;
using System.Collections.ObjectModel;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfViewpointViewModel : BindableBase
  {
    private Guid _id = Guid.NewGuid();
    private byte[] _snapshot;
    private BcfViewpointCameraViewModelBase _camera;

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

    public ObservableCollection<BcfViewpointLineViewModel> Lines { get; } = new ObservableCollection<BcfViewpointLineViewModel>();
    public ObservableCollection<BcfViewpointComponentViewModel> Components { get; } = new ObservableCollection<BcfViewpointComponentViewModel>();
    public ObservableCollection<BcfViewpointClippingPlaneViewModel> ClippingPlanes { get; } = new ObservableCollection<BcfViewpointClippingPlaneViewModel>();

    public BcfViewpointPerspectiveCameraViewModel PerspectiveCamera
    {
      get => _camera as BcfViewpointPerspectiveCameraViewModel;
      set
      {
        if (SetProperty(ref _camera, value))
        {
          OnPropertyChanged(nameof(OrthogonalCamera));
        }
      }
    }

    public BcfViewpointOrthogonalCameraViewModel OrthogonalCamera
    {
      get => _camera as BcfViewpointOrthogonalCameraViewModel;
      set
      {
        if (SetProperty(ref _camera, value))
        {
          OnPropertyChanged(nameof(PerspectiveCamera));
        }
      }
    }
  }
}
