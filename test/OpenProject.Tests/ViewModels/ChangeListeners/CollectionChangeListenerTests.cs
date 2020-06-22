using OpenProject.Bcf;
using OpenProject.Shared.ViewModels.Bcf;
using OpenProject.Shared.ViewModels.ChangeListeners;
using System.Collections.ObjectModel;
using Xunit;

namespace OpenProject.Tests.ViewModels.ChangeListeners
{
  public class CollectionChangeListenerTests
  {
    [Fact]
    public void NotifiesOfChangeWhenListElementAdded()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      var changeListener = new CollectionChangeListener(collection);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      collection.Add(new BcfTopicViewModel());
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenListElementRemoved()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      collection.Add(new BcfTopicViewModel());
      var changeListener = new CollectionChangeListener(collection);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      collection.Clear();
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenPropertyInListElementChanged_WhenElementExistedBeforeListenerCreation()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      var topic = new BcfTopicViewModel();
      collection.Add(topic);
      var changeListener = new CollectionChangeListener(collection);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      topic.Title = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void NotifiesOfChangeWhenPropertyInListElementChanged_WhenElementAddedAfterListenerCreation()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      var topic = new BcfTopicViewModel();
      var changeListener = new CollectionChangeListener(collection);
      collection.Add(topic);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      topic.Title = "Hello World!";
      Assert.True(hasNotified);
    }

    [Fact]
    public void DoesNotNotifyOfChangeInRemovedListElement_WhenElementAddedBeforeListenerCreation()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      var topic = new BcfTopicViewModel();
      collection.Add(topic);
      var changeListener = new CollectionChangeListener(collection);
      collection.Remove(topic);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      topic.Title = "Hello World!";
      Assert.False(hasNotified);
    }

    [Fact]
    public void DoesNotNotifyOfChangeInRemovedListElement_WhenElementAddedAfterListenerCreation()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      var topic = new BcfTopicViewModel();
      var changeListener = new CollectionChangeListener(collection);
      collection.Add(topic);
      collection.Remove(topic);
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      topic.Title = "Hello World!";
      Assert.False(hasNotified);
    }

    [Fact]
    public void DoesNotNotifyOfChangeInRemovedListElement_WhenListCleared()
    {
      var collection = new ObservableCollection<BcfTopicViewModel>();
      var topic = new BcfTopicViewModel();
      var changeListener = new CollectionChangeListener(collection);
      collection.Add(topic);
      collection.Clear();
      var hasNotified = false;
      changeListener.PropertyChanged += (s, e) => hasNotified = true;
      topic.Title = "Hello World!";
      Assert.False(hasNotified);
    }
  }
}
