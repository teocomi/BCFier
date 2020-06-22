using OpenProject.Shared.ViewModels.Bcf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace OpenProject.Bcf
{
  public static class BcfLocalFileLoader
  {
    public static List<BcfFileViewModel> OpenFileDialogAndLoadBcfFile()
    {
      var bcffiles = OpenBcfDialog();
      return bcffiles;
    }

    public static BcfFileViewModel OpenLocalBcfFile(string filename)
    {
      using (var fs = File.OpenRead(filename))
      {
        var bcfFileViewModel = new BcfFileLoader(filename, fs).LoadFromBcf();
        return bcfFileViewModel;
      }
    }

    /// <summary>
    /// Prompts a dialog to select one or more BCF files to open
    /// </summary>
    /// <returns></returns>
    private static List<BcfFileViewModel> OpenBcfDialog()
    {
      var bcfFileViewModels = new List<BcfFileViewModel>();
      try
      {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog();
        openFileDialog.Filter = "BCF v2.1 (*.bcf)|*.bcf|BCF v2.0 (*.bcfzip)|*.bcfzip";
        openFileDialog.DefaultExt = ".bcfzip";
        openFileDialog.Multiselect = true;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        var result = openFileDialog.ShowDialog(); // Show the dialog.

        if (result == true) // Test result.
        {
          foreach (var bcfFileName in openFileDialog.FileNames)
          {
            var bcfFileViewModel = OpenLocalBcfFile(bcfFileName);
            bcfFileViewModels.Add(bcfFileViewModel);
          }
        }
      }
      catch (Exception e)
      {
        MessageBox.Show("Exception while trying to load BCF files: " + Environment.NewLine + e);
      }
      return bcfFileViewModels;
    }
  }
}
