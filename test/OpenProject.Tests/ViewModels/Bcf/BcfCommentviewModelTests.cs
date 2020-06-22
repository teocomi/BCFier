using OpenProject.Shared.ViewModels.Bcf;
using System;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfCommentviewModelTests
  {
    [Fact]
    public void InstantiatesWithRanomdGuidForId()
    {
      var actual = new BcfCommentviewModel().Id;
      Assert.NotEqual(Guid.Empty, actual);
    }

    [Fact]
    public void CanSetGuidAsId()
    {
      var viewModel = new BcfCommentviewModel();
      var guid = Guid.NewGuid();
      Assert.NotEqual(guid, viewModel.Id);
      viewModel.Id = guid;
      Assert.Equal(guid, viewModel.Id);
    }

    [Fact]
    public void ViewpointIdIsNullAfterInstantiation()
    {
      var viewModel = new BcfCommentviewModel();
      Assert.Null(viewModel.ViewpointId);
    }

    [Fact]
    public void InstantiatesWithCreationDateSetToUTcNow()
    {
      var viewModel = new BcfCommentviewModel();
      Assert.True((DateTime.UtcNow - viewModel.CreationDate).TotalMinutes <= 5);
    }
  }
}
