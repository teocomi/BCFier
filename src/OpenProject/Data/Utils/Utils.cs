using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenProject.Data.Utils
{
  public static class Utils
  {
    public static bool IsFileLocked(string path)
    {
      if (!File.Exists(path))
        return false;
      var file = new FileInfo(path);
      FileStream stream = null;
      try
      {
        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      }
      catch (IOException)
      {
        //the file is unavailable because it is:
        //still being written to
        //or being processed by another thread
        //or does not exist (has already been processed)
        return true;
      }
      finally
      {
        if (stream != null)
          stream.Close();
      }

      //file is not locked
      return false;
    }
    /// <summary>
    /// If no Username is set in settings returns the Windows username
    /// </summary>
    /// <returns></returns>
    public static string GetUsername()
    {
      string username = UserSettings.Get("BCFusername");

      if (string.IsNullOrEmpty(username)) //EMPTY VALUE IN CONFIG
      {
        string s = WindowsIdentity.GetCurrent().Name;
        int stop = s.IndexOf("\\");
        username = (stop > -1) ? s.Substring(stop + 1, s.Length - stop - 1) : "Unknown User";
      }
      return username;
    }

    /// <summary>
    /// Recursively deletes safely a directory and its content
    /// </summary>
    /// <param name="target_dir"></param>
      public static void DeleteDirectory(string target_dir)
      {
        try
        {
          if (Directory.Exists(target_dir))
          {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);
            foreach (string file in files)
            {
              File.SetAttributes(file, FileAttributes.Normal);
              File.Delete(file);
            }

            foreach (string dir in dirs)
            {
              DeleteDirectory(dir);
            }
            Directory.Delete(target_dir, false);
          }
        }
        catch (System.Exception ex1)
        {
          MessageBox.Show("exception: " + ex1);
        }
      }
    
  }
}
