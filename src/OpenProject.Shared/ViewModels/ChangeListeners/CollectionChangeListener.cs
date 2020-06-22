using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OpenProject.Shared.ViewModels.ChangeListeners
{
  public class CollectionChangeListener : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    private Dictionary<object, ChangeListener> _changeListenersByCollectionElement = new Dictionary<object, ChangeListener>();

    public bool ChildModified { get; private set; }

    public CollectionChangeListener(INotifyCollectionChanged notifyCollectionChanged)
    {
      notifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;

      var enumerable = notifyCollectionChanged as IEnumerable<INotifyPropertyChanged>;
      if (enumerable != null)
      {
        foreach (var element in enumerable)
        {
          var childChangeListener = new ChangeListener(element);
          childChangeListener.PropertyChanged += ChildChangeListener_PropertyChanged;
          _changeListenersByCollectionElement.Add(element, childChangeListener);
        }
      }
    }

    private void NotifyOfChange()
    {
      ChildModified = true;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeListener.ChildModified)));
    }

    private void ChildChangeListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      NotifyOfChange();
    }

    private void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      var hasAddedOrRemoved = false;
      if (e.NewItems?.Count > 0)
      {
        foreach (var newItem in e.NewItems)
        {
          if (newItem is INotifyPropertyChanged propertyChanged)
          {
            hasAddedOrRemoved = true;
            var childChangeListener = new ChangeListener(propertyChanged);
            childChangeListener.PropertyChanged += ChildChangeListener_PropertyChanged;
            _changeListenersByCollectionElement.Add(propertyChanged, childChangeListener);
          }
        }
      }
      if (e.OldItems?.Count > 0)
      {
        foreach (var oldItem in e.OldItems)
        {
          if (oldItem is INotifyPropertyChanged propertyChanged)
          {
            if (_changeListenersByCollectionElement.ContainsKey(oldItem))
            {
              hasAddedOrRemoved = true;
              _changeListenersByCollectionElement[oldItem].PropertyChanged -= ChildChangeListener_PropertyChanged;
              _changeListenersByCollectionElement.Remove(oldItem);
            }
          }
        }
      }
      if (e.Action == NotifyCollectionChangedAction.Move
        || e.Action == NotifyCollectionChangedAction.Reset)
      {
        hasAddedOrRemoved = true;
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
          foreach (var oldListener in _changeListenersByCollectionElement.Values)
          {
            oldListener.PropertyChanged -= ChildChangeListener_PropertyChanged;
          }
          _changeListenersByCollectionElement.Clear();
        }
      }

      if (hasAddedOrRemoved)
      {
        NotifyOfChange();
      }
    }
  }
}
