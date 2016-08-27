using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

using System.IO;
using Bcfier.Data.Utils;
using Bcfier.Navisworks.Data;
using Bcfier.Bcf.Bcf2;
using Autodesk.Navisworks.Api;
using Bcfier.Navisworks.Windows;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;


namespace Bcfier.Navisworks
{
  /// <summary>
  /// Interaction logic for NavisWindow.xaml
  /// </summary>
  public partial class NavisWindow : UserControl
  {
    List<SavedViewpoint> _savedViewpoints = new List<SavedViewpoint>();

    public NavisWindow()
    {
      InitializeComponent();

      // set image export settings
      // configure the option "export.image.format" to export png and image size
      ComApi.InwOaPropertyVec options = ComBridge.State.GetIOPluginOptions("lcodpimage");
      foreach (ComApi.InwOaProperty opt in options.Properties())
      {
        if (opt.name == "export.image.format")
          opt.value = "lcodpexpng";
        if (opt.name == "export.image.width")
          opt.value = 1600;
        if (opt.name == "export.image.height")
          opt.value = 900;
      }
    }
    #region commands
    /// <summary>
    /// Raised when opening a view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOpenView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        if (Bcfier.SelectedBcf() == null)
          return;
        var view = e.Parameter as ViewPoint;
        if (view == null)
          return;

        var v = view.VisInfo;
        
        //current document
        var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        NavisView.Open(doc,v);
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error opening a View!");
      }
    }
    /// <summary>
    /// Same as in the windows app, but here we generate a VisInfo that is attached to the view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnAddView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {

        if (Bcfier.SelectedBcf() == null)
          return;
        var issue = e.Parameter as Markup;
        if (issue == null)
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        try
        {
          var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
          var dialog = new AddViewNavis(issue, Bcfier.SelectedBcf().TempPath);
          dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          dialog.ShowDialog();
          if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
          {
            //generate and set the VisInfo
            //issue.Viewpoints.Last().VisInfo = GenerateViewpoint();

            //set filename here as it's not set when an issue is created
            if (!string.IsNullOrEmpty(doc.FileName))
              issue.Header[0].Filename = doc.FileName;
            else
              issue.Header[0].Filename = "Unknown";

            Bcfier.SelectedBcf().HasBeenSaved = false;
          }
        }

        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
        }

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error adding a View!");
      }
    }

    private void OnAddIssues(object sender, ExecutedRoutedEventArgs e)
    {

    }

   

    private void RecurseItems(SavedItem savedItem)
    {
      try
      {
        Autodesk.Navisworks.Api.GroupItem group = savedItem as Autodesk.Navisworks.Api.GroupItem;
        //is a group
        if (null != group)
        {
          foreach (SavedItem child in group.Children)
          {
            RecurseItems(child);
          }
        }
        else
        {
          _savedViewpoints.Add((SavedViewpoint)savedItem);
        }
      }
      catch
      {
       //do nothing
      }

    }


    #endregion
    //stats
    private void NavisWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      Task.Run(() =>
      {
        StatHat.Post.EzCounter(@"hello@teocomi.com", "BCFierNavisStart", 1);
      });
    }
  }
}
