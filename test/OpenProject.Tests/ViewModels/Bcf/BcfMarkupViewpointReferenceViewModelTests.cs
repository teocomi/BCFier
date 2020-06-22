using OpenProject.Shared.ViewModels.Bcf;
using System;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfMarkupViewpointReferenceViewModelTests
  {
    [Fact]
    public void InstantiatesWithRanomdGuidForId()
    {
      var actual = new BcfMarkupViewpointReferenceViewModel().Id;
      Assert.NotEqual(Guid.Empty, actual);
    }

    [Fact]
    public void CanSetGuidAsId()
    {
      var viewModel = new BcfMarkupViewpointReferenceViewModel();
      var guid = Guid.NewGuid();
      Assert.NotEqual(guid, viewModel.Id);
      viewModel.Id = guid;
      Assert.Equal(guid, viewModel.Id);
    }
  }
}
