using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bcfier.Bcf.Bcf2;
using Bcfier.Data.Utils;

namespace Bcfier.UserControls
{
  /// <summary>
  /// Control used to add views
  /// I wanted to extand a window insted than embedding a User Control but if was giving problems
  /// </summary>
  public partial class AddView : UserControl
  {
    internal string TempFolder;
    internal Markup Issue;

    public AddView()
    {
      InitializeComponent();
    }

    public AddView(Markup issue, string bcfTempFolder)
    {
      try
      {
        InitializeComponent();
        Issue = issue;
        TempFolder = bcfTempFolder;
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    #region events
    private void Button_RemoveImage(object sender, RoutedEventArgs e)
    {
      SnapshotImg.Source = null;
    }

    //LOAD EXTERNAL IMAGE
    private void Button_LoadImage(object sender, RoutedEventArgs e)
    {

      var dialog = new Microsoft.Win32.OpenFileDialog
      {
        Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png",
        DefaultExt = ".png",
        CheckFileExists = true,
        CheckPathExists = true,
        RestoreDirectory = true
      };
      var result = dialog.ShowDialog(); // Show the dialog.

      if (result != true) // Test result.
        return;

      AddViewpoint(dialog.FileName);
    }

    internal void AddViewpoint(string imagePath)
    {
      SnapshotImg.Source = ImagingUtils.ImageSourceFromPath(imagePath);
    }

    private void Button_Cancel(object sender, RoutedEventArgs e)
    {
      var win = Window.GetWindow(this);
      if(win!=null)
        win.DialogResult = false;
    }

    private void Button_OK(object sender, RoutedEventArgs e)
    {
      var view = new ViewPoint(!Issue.Viewpoints.Any());

      if (!Directory.Exists(Path.Combine(TempFolder, Issue.Topic.Guid)))
        Directory.CreateDirectory(Path.Combine(TempFolder, Issue.Topic.Guid));


      if (!string.IsNullOrEmpty(CommentBox.Text))
      {
        var c = new Comment
        {
          Comment1 = CommentBox.Text,
          Author = Utils.GetUsername(),
          //Status = comboStatuses.SelectedValue.ToString(),
          //VerbalStatus = VerbalStatus.Text,
          Date = DateTime.Now,
          Viewpoint = new CommentViewpoint { Guid = view.Guid }
        };
        Issue.Comment.Add(c);
      }
      //first save the image, then update path
      var path = Path.Combine(TempFolder, Issue.Topic.Guid, view.Snapshot);
      ImagingUtils.SaveImageSource(SnapshotImg.Source, path);
      view.SnapshotPath = path;

      //neede for UI binding
      Issue.Viewpoints.Add(view);

      var win = Window.GetWindow(this);
      if (win != null)
        win.DialogResult = true;
    }

    private void EditSnapshot_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string editSnap = "mspaint";
        string tempImg = Path.Combine(Path.GetTempPath(), "BCFier", Path.GetTempFileName() + ".png");
        ImagingUtils.SaveImageSource(SnapshotImg.Source, tempImg);

        if (!File.Exists(tempImg))
        {
          MessageBox.Show("Snapshot is not a valit image, please try again.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        string customeditor = UserSettings.Get("editSnap");
        if (!string.IsNullOrEmpty(customeditor) && File.Exists(customeditor))
          editSnap = customeditor;

       

        var paint = new Process();
        var paintInfo = new ProcessStartInfo(editSnap, "\"" + tempImg + "\"");
        paintInfo.UseShellExecute = false;
        paint.StartInfo = paintInfo;
        paint.Start();
        paint.WaitForExit();


        AddViewpoint(tempImg);
        File.Delete(tempImg);




      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    private bool IsProcessOpen(string name)
    {
      foreach (Process clsProcess in Process.GetProcesses())
      {

        if (clsProcess.ProcessName.Contains(name))
        {
          return true;
        }
      }
      //otherwise we return a false
      return false;
    }
    #endregion

    #region drag&drop
    private void Window_DragEnter(object sender, DragEventArgs e)
    {
      whitespace.Visibility = Visibility.Visible;
    }
    private void Window_DragLeave(object sender, DragEventArgs e)
    {
      whitespace.Visibility = Visibility.Hidden;
    }
    private void Window_Drop(object sender, DragEventArgs e)
    {
      try
      {
        whitespace.Visibility = Visibility.Hidden;
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
          return;
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (!files.Any() || !File.Exists(files.First()))
          return;
        AddViewpoint(files.First());
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    private void Window_DragOver(object sender, DragEventArgs e)
    {
      try
      {
        var extensions = new List<string> { ".jpg", ".jpeg", ".gif", ".bmp", ".png", ".tif" };
        var dropEnabled = true;

        if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
        {
          var filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
          if (filenames.Count() != 1 || !extensions.Contains(Path.GetExtension(filenames.First()).ToLowerInvariant()))
            dropEnabled = false;
        }
        else
          dropEnabled = false;

        if (!dropEnabled)
        {
          e.Effects = DragDropEffects.None;
          e.Handled = true;
        }
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    #endregion

  }
}
