using OpenProject.Shared.ViewModels.Bcf;
using System;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfViewpointViewModelTests
  {
    [Fact]
    public void InstantiatesWithRanomdGuidForId()
    {
      var actual = new BcfViewpointViewModel().Id;
      Assert.NotEqual(Guid.Empty, actual);
    }

    [Fact]
    public void CanSetGuidAsId()
    {
      var viewModel = new BcfViewpointViewModel();
      var guid = Guid.NewGuid();
      Assert.NotEqual(guid, viewModel.Id);
      viewModel.Id = guid;
      Assert.Equal(guid, viewModel.Id);
    }

    [Fact]
    public void HasNoCameraAtInstantiation()
    {
      var viewModel = new BcfViewpointViewModel();
      Assert.Null(viewModel.PerspectiveCamera);
      Assert.Null(viewModel.OrthogonalCamera);
    }

    [Fact]
    public void SetsOtherCameraToNullWhenSettingCamera()
    {
      var viewModel = new BcfViewpointViewModel();
      viewModel.PerspectiveCamera = new BcfViewpointPerspectiveCameraViewModel();
      Assert.Null(viewModel.OrthogonalCamera);
      Assert.NotNull(viewModel.PerspectiveCamera);
      viewModel.OrthogonalCamera = new BcfViewpointOrthogonalCameraViewModel();
      Assert.Null(viewModel.PerspectiveCamera);
      Assert.NotNull(viewModel.OrthogonalCamera);
      viewModel.PerspectiveCamera = new BcfViewpointPerspectiveCameraViewModel();
      Assert.Null(viewModel.OrthogonalCamera);
      Assert.NotNull(viewModel.PerspectiveCamera);
    }
  }
}
