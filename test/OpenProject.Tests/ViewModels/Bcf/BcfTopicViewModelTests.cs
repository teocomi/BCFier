using OpenProject.Shared.ViewModels.Bcf;
using System;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfTopicViewModelTests
  {
    [Fact]
    public void InstantiatesWithRanomdGuidForId()
    {
      var actual = new BcfTopicViewModel().Id;
      Assert.NotEqual(Guid.Empty, actual);
    }

    [Fact]
    public void CanSetGuidAsId()
    {
      var viewModel = new BcfTopicViewModel();
      var guid = Guid.NewGuid();
      Assert.NotEqual(guid, viewModel.Id);
      viewModel.Id = guid;
      Assert.Equal(guid, viewModel.Id);
    }

    [Fact]
    public void InstantiatesWithCreationDateSetToUTcNow()
    {
      var viewModel = new BcfTopicViewModel();
      Assert.True((DateTime.UtcNow - viewModel.CreationDate).TotalMinutes <= 5);
    }

    [Fact]
    public void InstantiatesWithEmptyListOfLabels()
    {
      var viewModel = new BcfTopicViewModel();
      Assert.NotNull(viewModel.Labels);
      Assert.Empty(viewModel.Labels);
    }
  }
}
