using OpenProject.Shared.ViewModels.Bcf;
using System;
using System.IO;
using System.Windows;

namespace OpenProject.Bcf
{
  public static class BcfLocalFileSaver
  {
    public static void SaveBcfFileLocally(BcfFileViewModel bcfFile)
    {
      if (bcfFile.BcfIssues.Count == 0)
      {
        MessageBox.Show("The current BCF Report is empty.", "No Issue", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      // Show save file dialog box
      string name = !string.IsNullOrEmpty(bcfFile.FileName)
          ? bcfFile.FileName
          : "New BCF Report";
      string filename = SaveBcfDialog(name);
      SaveBcfFile(bcfFile, filename);
    }

    /// <summary>
    /// Prompts a the user to select where to save the bcfzip
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private static string SaveBcfDialog(string filename)
    {
      var saveFileDialog = new Microsoft.Win32.SaveFileDialog
      {
        Title = "Save as BCF report file (.bcfzip, -bcf)",
        FileName = filename,
        DefaultExt = ".bcfzip",
        Filter = "BCF v2.0 (*.bcfzip)|*.bcfzip|BCF v2.1 (*.bcf)|*.bcf"
      };

      //if it goes fine I return the filename, otherwise empty
      var result = saveFileDialog.ShowDialog();
      return result == true ? saveFileDialog.FileName : "";
    }

    /// <summary>
    /// Serializes to a bcfzip and saves it to disk
    /// </summary>
    /// <param name="bcffile"></param>
    /// <returns></returns>
    private static bool SaveBcfFile(BcfFileViewModel bcfFile, string filename)
    {
      try
      {
        // Process save file dialog box results
        if (string.IsNullOrWhiteSpace(filename))
          return false;

        var bcfVersion = filename.EndsWith(".bcf", StringComparison.InvariantCultureIgnoreCase)
          ? BcfVersion.V21 // .bcf is the extension for version 2.1
          : BcfVersion.V20;

        //overwrite, without doubts
        if (File.Exists(filename))
          File.Delete(filename);

        using (var fs = File.Create(filename))
        {
          var bcfConverter = new BcfFileConverter(bcfFile);
          using (var bcfStream = bcfConverter.GetBcfFileStream(bcfVersion))
          {
            bcfStream.CopyTo(fs);
          }
        }

        //Open browser at location
        var fileUri = new Uri(filename);
        var bcfExportName = Path.GetFileName(fileUri.LocalPath);

        if (File.Exists(filename))
        {
          string argument = @"/select, " + filename;
          System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        bcfFile.FileName = bcfExportName;
        bcfFile.IsModified = false;
      }
      catch (Exception e)
      {
        MessageBox.Show("Exception during file save:" + Environment.NewLine + e);
      }
      return true;
    }
  }
}
