using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using OpenProject.Data.Utils;
using OpenProject.Data;

namespace OpenProject.Windows
{
  /// <summary>
  /// Interaction logic for Settings.xaml
  /// </summary>
  public partial class Settings : Window
  {
    /// <summary>
    /// All controls in this list will be automatically saved and their values loaded when the window is loaded
    /// </summary>
    private List<Control> ControlsToSave = new List<Control>();

    public Settings()
    {
      InitializeComponent();
      ControlsToSave = new List<Control> { BCFusername, checkupdates, editSnap, useDefPhoto, Stauses, Types, alwaysNewView, OpenProjectAccessToken, OpenProjectBaseUrl };
      foreach (var control in ControlsToSave)
        UserSettings.LoadControlSettings(control);

    }

    private void SaveBtnClick(object sender, RoutedEventArgs e)
    {
      foreach (var control in ControlsToSave)
        UserSettings.SaveControlSettings(control);

      DialogResult = true;
    }
    private void CancelBtnClick(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void editphoto_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var initDir = "";
      if (File.Exists(editSnap.Text))
        initDir = Path.GetDirectoryName(editSnap.Text);
      var openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
      openFileDialog1.Filter = "Executable (*.exe)|*.exe";
      openFileDialog1.DefaultExt = ".exe";
      openFileDialog1.InitialDirectory = (string.IsNullOrEmpty(initDir)) ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) : initDir;
      openFileDialog1.RestoreDirectory = true;
      var result = openFileDialog1.ShowDialog(); // Show the dialog.

      if (result == true) // Test result.
      {
        editSnap.Text = openFileDialog1.FileName;
      }
    }
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      try
      {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      }
      catch (Win32Exception)
      {
        Process.Start("IExplore.exe", e.Uri.AbsoluteUri);
      }

      e.Handled = true;
    }




  }
}
