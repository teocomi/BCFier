using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace OpenProject.Shared.ViewModels.ChangeListeners
{
  public class ChangeListener : INotifyPropertyChanged
  {
    public INotifyPropertyChanged NotifyPropertyChanged { get; }

    public ChangeListener(INotifyPropertyChanged notifyPropertyChanged)
    {
      NotifyPropertyChanged = notifyPropertyChanged;
      NotifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;

      var childProperties = notifyPropertyChanged.GetType()
        .GetProperties()
        .Where(cp => cp.PropertyType.GetInterfaces().Any(i => i == typeof(INotifyPropertyChanged)))
        .Where(cp => cp.PropertyType.GetInterfaces().All(i => i != typeof(INotifyCollectionChanged)))
        .ToList();
      foreach (var childProperty in childProperties)
      {
        if (childProperty.PropertyType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
        {
          var propertyValue = childProperty.GetValue(notifyPropertyChanged) as INotifyPropertyChanged;
          if (propertyValue != null)
          {
            ListenToChangesInChild(childProperty.Name, propertyValue);
          }
        }
      }

      var childCollections = NotifyPropertyChanged.GetType()
        .GetProperties()
        .Where(cp => cp.PropertyType.GetInterfaces().Any(i => i == typeof(INotifyCollectionChanged)))
        .ToList();
      foreach (var childCollection in childCollections)
      {
        var childCollectionChanged = childCollection.GetValue(notifyPropertyChanged) as INotifyCollectionChanged;
        if (childCollectionChanged != null)
        {
          var collectionChangeListener = new CollectionChangeListener(childCollectionChanged);
          _collectionChangeListenersByPropertyName.Add(childCollection.Name, collectionChangeListener);
          collectionChangeListener.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChildModified)));
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public bool ChildModified { get; private set; }
    private readonly Dictionary<string, ChangeListener> _changeListenersByPropertyName = new Dictionary<string, ChangeListener>();
    private readonly Dictionary<string, CollectionChangeListener> _collectionChangeListenersByPropertyName = new Dictionary<string, CollectionChangeListener>();

    private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      ChildModified = true;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChildModified)));

      if (e.PropertyName == nameof(ChildModified))
      {
        return;
      }

      var propertyValue = sender.GetType()
        .GetProperties()
        .FirstOrDefault(p => p.Name == e.PropertyName)
        .GetValue(sender);

      if (propertyValue is INotifyPropertyChanged notifiyPropertyChanged)
      {
        ListenToChangesInChild(e.PropertyName, notifiyPropertyChanged);
      }
    }

    private void ListenToChangesInChild(string propertyName, INotifyPropertyChanged childPropertyValue)
    {
      if (_changeListenersByPropertyName.ContainsKey(propertyName)
        && _changeListenersByPropertyName[propertyName].NotifyPropertyChanged != childPropertyValue)
      {
        _changeListenersByPropertyName[propertyName].PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
        var newListener = new ChangeListener(childPropertyValue);
        newListener.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
        _changeListenersByPropertyName[propertyName] = newListener;
      }
      else if (!_changeListenersByPropertyName.ContainsKey(propertyName))
      {
        var newListener = new ChangeListener(childPropertyValue);
        newListener.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
        _changeListenersByPropertyName.Add(propertyName, newListener);
      }
    }
  }
}
