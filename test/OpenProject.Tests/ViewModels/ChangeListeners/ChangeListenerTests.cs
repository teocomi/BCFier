using OpenProject.Bcf;
using OpenProject.Shared.ViewModels.Bcf;
using OpenProject.Shared.ViewModels.ChangeListeners;
using System.Linq;
using Xunit;

namespace OpenProject.Tests.ViewModels.ChangeListeners
{
  public class ChangeListenerTests
  {
    [Fact]
    public void NotifiesOfChangeWhenElementAdded()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.BcfTopic = new BcfTopicViewModel();
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenBcfTopicTitleChanged()
    {
      var bcfTopic = new BcfTopicViewModel();
      var changeListener = new ChangeListener(bcfTopic);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfTopic.Title = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenPropertyInChildIsChanged_WhenChangeListenerCreatedAfterPropertyIsAssigned()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      bcfMarkup.BcfTopic = new BcfTopicViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.BcfTopic.Title = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenPropertyInChildIsChanged_WhenChangeListenerCreatedBeforePropertyIsAssigned()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      bcfMarkup.BcfTopic = new BcfTopicViewModel();
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.BcfTopic.Title = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenListElementAdded()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.Comments.Add(new BcfCommentviewModel());
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenListPropertyRemoved()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      bcfMarkup.Comments.Add(new BcfCommentviewModel());
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.Comments.Clear();
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenPropertyInListElementChanged_WhenElementPresentBeforeListenerCreated()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      bcfMarkup.Comments.Add(new BcfCommentviewModel());
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.Comments.Single().Text = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenPropertyInListElementChanged_WhenElementPresentAfterListenerCreated()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      bcfMarkup.Comments.Add(new BcfCommentviewModel());
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      bcfMarkup.Comments.Single().Text = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void DoesNotNotifyOfChangeInRemovedListElement()
    {
      var bcfMarkup = new BcfMarkupViewModel();
      var changeListener = new ChangeListener(bcfMarkup);
      var hasNotified = false;
      var comment = new BcfCommentviewModel();
      bcfMarkup.Comments.Add(comment);
      bcfMarkup.Comments.Clear();
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      comment.Text = "Hello World!";
      Assert.False(hasNotified);
    }
  }
}
