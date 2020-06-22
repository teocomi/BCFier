using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenProject.Revit.Entry
{
  public static class ConfigurationLoader
  {
    public static string GetBcfierWinExecutablePath()
    {
      var configurationFilePath = GetConfigurationFilePath();
      using (var fs = File.OpenRead(configurationFilePath))
      {
        using (var sr = new StreamReader(fs))
        {
          var json = sr.ReadToEnd();
          var jObject = JObject.Parse(json);
          var bcfierWinExecutablePath = jObject["OpenProjectWindowsExecutablePath"].ToString();

          if (!System.IO.Path.IsPathRooted(bcfierWinExecutablePath))
          {
            var currentFolder = GetCurrentDllDirectory();
            bcfierWinExecutablePath = Path.Combine(currentFolder, bcfierWinExecutablePath);
          }

          if (!File.Exists(bcfierWinExecutablePath))
          {
            throw new Exception($"The OpenProject.Windows.exe path in the configuration is given as: \"{bcfierWinExecutablePath}\", but the file could not be found.");
          }

          return bcfierWinExecutablePath;
        }
      }
    }

    private static string GetConfigurationFilePath()
    {
      var configPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "OpenProject.Revit",
        "OpenProject.Revit.Configuration.json");

      if (!File.Exists(configPath))
      {
        // If the file doesn't yet exist, the default one is created
        using (var configStream = typeof(ConfigurationLoader).Assembly.GetManifestResourceStream("OpenProject.Revit.OpenProject.Revit.Configuration.json"))
        {
          using (var fs = File.Create(configPath))
          {
            configStream.CopyTo(fs);
          }
        }
      }

      return configPath;
    }

    private static string GetCurrentDllDirectory()
    {
      var currentAssemblyPathUri = Assembly.GetExecutingAssembly().CodeBase;
      var currentAssemblyPath = Uri.UnescapeDataString(new Uri(currentAssemblyPathUri).AbsolutePath)
        // '/' comes from the uri, we need it to be '\' for the path
        .Replace("/", "\\");
      var currentFolder = Path.GetDirectoryName(currentAssemblyPath);
      return currentFolder;
    }
  }
}