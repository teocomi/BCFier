using OpenProject.Shared.ViewModels.Bcf;
using System;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfFileViewModelTests
  {
    [Fact]
    public void InstantiatesWithRanomdGuidForId()
    {
      var actual = new BcfFileViewModel().Id;
      Assert.NotEqual(Guid.Empty, actual);
    }

    [Fact]
    public void CanSetGuidAsId()
    {
      var viewModel = new BcfFileViewModel();
      var guid = Guid.NewGuid();
      Assert.NotEqual(guid, viewModel.Id);
      viewModel.Id = guid;
      Assert.Equal(guid, viewModel.Id);
    }

    [Fact]
    public void DefaultsToUnknownBcfVersion()
    {
      var actual = new BcfFileViewModel().BcfVersion;
      Assert.Equal(BcfVersion.Unknown, actual);
    }

    [Fact]
    public void BcfIssuesFilteredIsInstantiatedAtStart()
    {
      Assert.NotNull(new BcfFileViewModel().BcfIssuesFiltered);
      Assert.Empty(new BcfFileViewModel().BcfIssuesFiltered);
    }

    [Fact]
    public void BcfIssuesIsInstantiatedAtStart()
    {
      Assert.NotNull(new BcfFileViewModel().BcfIssues);
      Assert.Empty(new BcfFileViewModel().BcfIssues);
    }

    [Fact]
    public void ReturnsUnfilteredTopicListIfNoFilter()
    {
      var bcfFile = new BcfFileViewModel();
      bcfFile.BcfIssues.Add(new BcfIssueViewModel());
      Assert.Single(bcfFile.BcfIssues);
      Assert.Single(bcfFile.BcfIssuesFiltered);
    }

    [Fact]
    public void FiltersTopicsWhenTextFilterPropertyIsChangedOrTopicIsAdded()
    {
      var bcfFile = new BcfFileViewModel();
      bcfFile.BcfIssues.Add(new BcfIssueViewModel
      {
        Markup = new BcfMarkupViewModel
        {
          BcfTopic = new BcfTopicViewModel
          {
            Title = "Hello World!"
          }
        }
      });
      Assert.Single(bcfFile.BcfIssues);
      Assert.Single(bcfFile.BcfIssuesFiltered);

      bcfFile.TextSearch = "BCF";

      Assert.Single(bcfFile.BcfIssues);
      Assert.Empty(bcfFile.BcfIssuesFiltered);

      bcfFile.BcfIssues.Add(new BcfIssueViewModel
      {
        Markup = new BcfMarkupViewModel
        {
          BcfTopic = new BcfTopicViewModel
          {
            Title = "BCF Topic"
          }
        }
      });

      Assert.Equal(2, bcfFile.BcfIssues.Count);
      Assert.Single(bcfFile.BcfIssuesFiltered);

      bcfFile.TextSearch = null;

      Assert.Equal(2, bcfFile.BcfIssues.Count);
      Assert.Equal(2, bcfFile.BcfIssuesFiltered.Count);
    }
  }
}
