using OpenProject.Shared.ViewModels.Bcf;
using System.Linq;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfIssueViewModelTests
  {
    [Fact]
    public void SetsIsModifiedWhenTopicIsAdded()
    {
      var issue = new BcfIssueViewModel();
      Assert.False(issue.IsModified);
      issue.Markup = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };

      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup.BcfTopic.ModifiedDate);
    }

    [Fact]
    public void DoesNotUpdateIsChangedOnReleasedProperty()
    {
      var issue = new BcfIssueViewModel();
      Assert.False(issue.IsModified);
      var markupOld = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };
      issue.Markup = markupOld;
      issue.Markup = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };
      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup.BcfTopic.ModifiedDate);

      var originalModifiedDate = issue.Markup.BcfTopic.ModifiedDate.Value;

      markupOld.BcfTopic.Title = "New Title";
      Assert.Equal(originalModifiedDate, issue.Markup.BcfTopic.ModifiedDate.Value);
    }

    [Fact]
    public void SetsIsModifiedWhenValueInsideIsModified()
    {
      var issue = new BcfIssueViewModel();
      issue.DisableListeningForChanges = true;
      issue.Markup = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };
      issue.DisableListeningForChanges = false;

      Assert.False(issue.IsModified);
      issue.Markup.BcfTopic.Title = "Changed Title";
      Assert.True(issue.IsModified);
    }

    [Fact]
    public void UpdatesModifiedDateWhenValueIsModified_InTopic()
    {
      var issue = new BcfIssueViewModel();
      issue.DisableListeningForChanges = true;
      issue.Markup = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };
      issue.DisableListeningForChanges = false;

      Assert.False(issue.IsModified);
      Assert.Null(issue.Markup.BcfTopic.ModifiedDate);
      issue.Markup.BcfTopic.Title = "Changed Title";
      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup.BcfTopic.ModifiedDate);
    }

    [Fact]
    public void UpdatesModifiedDateWhenValueIsModified_CommentAdded()
    {
      var issue = new BcfIssueViewModel();
      issue.DisableListeningForChanges = true;
      issue.Markup = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };
      issue.DisableListeningForChanges = false;

      Assert.False(issue.IsModified);
      Assert.Null(issue.Markup.BcfTopic.ModifiedDate);
      issue.Markup.Comments.Add(new BcfCommentviewModel
      {
        Text = "New Comment"
      });
      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup.BcfTopic.ModifiedDate);
    }

    [Fact]
    public void UpdatesModifiedDateWhenValueIsModified_CommentTextChanged()
    {
      var issue = new BcfIssueViewModel();
      issue.DisableListeningForChanges = true;
      issue.Markup = new BcfMarkupViewModel
      {
        BcfTopic = new BcfTopicViewModel()
      };
      issue.Markup.Comments.Add(new BcfCommentviewModel
      {
        Text = "Hello World!"
      });
      issue.DisableListeningForChanges = false;

      Assert.False(issue.IsModified);
      Assert.Null(issue.Markup?.BcfTopic?.ModifiedDate);
      issue.Markup.Comments.Single().Text = "Hello Beautiful World!";
      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup.BcfTopic.ModifiedDate);
    }

    [Fact]
    public void UpdatesModifiedDateWhenValueIsModified_ViewpointAdded()
    {
      var issue = new BcfIssueViewModel();
      Assert.False(issue.IsModified);
      Assert.Null(issue.Markup?.BcfTopic?.ModifiedDate);
      issue.Viewpoints.Add(new BcfViewpointViewModel());
      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup?.BcfTopic?.ModifiedDate);
    }

    [Fact]
    public void DoesNotUpdateWhenMarkupSetToNullAndChangedThen()
    {
      var issue = new BcfIssueViewModel();
      issue.DisableListeningForChanges = true;
      var markup = new BcfMarkupViewModel();
      issue.Markup = markup;
      issue.DisableListeningForChanges = false;
      Assert.False(issue.IsModified);
      Assert.Null(issue.Markup?.BcfTopic?.ModifiedDate);
      issue.Markup = null;
      Assert.True(issue.IsModified);
      Assert.NotNull(issue.Markup.BcfTopic.ModifiedDate);
      var expectedModifiedDate = issue.Markup.BcfTopic.ModifiedDate;
      markup.Comments.Add(new BcfCommentviewModel());
      Assert.Equal(expectedModifiedDate, issue.Markup.BcfTopic.ModifiedDate);
    }
  }
}
