using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Bcfier.Bcf;
using Bcfier.Themes;
using GongSolutions.Wpf.DragDrop;


namespace Bcfier.UserControls
{
  /// <summary>
  /// ViewModel for a BcfFile
  /// </summary>
  public partial class BcfReportPanel : UserControl
  {

    public BcfReportPanel()
    {
      InitializeComponent();

      //dummy call needed to have the dll properly load
      var d =  GongSolutions.Wpf.DragDrop.DragDrop.DataFormat;
      //binding set from code-behind
      //so that in the designer it still binds to the "Issues" collection
      //allowing for Design time preview
      //the binding to View is needed for filtering the collection
      IssueList.SetBinding(ItemsControl.ItemsSourceProperty, "View");
      ((INotifyCollectionChanged)IssueList.Items).CollectionChanged += IssueList_CollectionChanged;

     
    }

    private void IssueList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        // scroll the new item into view   
        IssueList.ScrollIntoView(e.NewItems[0]);
        TextBox_Title.Focus();
      }
    }

    private void TextBox_OnTextChanged(object sender, DataTransferEventArgs e)
    {
      if (IssueList.SelectedIndex == -1)
        return;
      var bcf = this.DataContext as BcfFile;
      if (bcf == null)
        return;
      //if (e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Left && e.Key != Key.Right)
      bcf.HasBeenSaved = false;
    }

    private void BcfReportPanel_OnLoaded(object sender, RoutedEventArgs e)
    {
    }


    private void AddCommentBtnClick(object sender, RoutedEventArgs e)
    {
      if (ViewCommList.Items.Count == 0)
        return;
      var item = ViewCommList.Items.GetItemAt(ViewCommList.Items.Count - 1);
      if (item == null)
        return;
      ViewCommList.ScrollIntoView(item);


      var lvi = (ListViewItem)ViewCommList.ItemContainerGenerator.ContainerFromItem(item);
      var tb = FindByName("TextBoxComment", lvi) as TextPlaceholder;

      if (tb != null)
        tb.Dispatcher.BeginInvoke(new Func<bool>(tb.Focus));

    }
   

    private FrameworkElement FindByName(string name, FrameworkElement root)
    {
      var tree = new Stack<FrameworkElement>();
      tree.Push(root);

      while (tree.Count > 0)
      {
        FrameworkElement current = tree.Pop();
        if (current.Name == name)
          return current;

        int count = VisualTreeHelper.GetChildrenCount(current);
        for (int i = 0; i < count; ++i)
        {
          DependencyObject child = VisualTreeHelper.GetChild(current, i);
          if (child is FrameworkElement)
            tree.Push((FrameworkElement)child);
        }
      }

      return null;
    }

    private void SearchBox_OnKeyDown(object sender, KeyEventArgs e)
    {
     if(e.Key== Key.Enter)
       Keyboard.ClearFocus();
    }
  }
}
