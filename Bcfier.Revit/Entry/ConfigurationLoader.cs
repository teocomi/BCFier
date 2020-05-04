using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;

namespace Bcfier.Revit.Entry
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
          var bcfierWinExecutablePath = jObject["BcfierWinExecutablePath"].ToString();

          if (!System.IO.Path.IsPathRooted(bcfierWinExecutablePath))
          {
            var currentFolder = GetCurrentDllDirectory();
            bcfierWinExecutablePath = Path.Combine(currentFolder, bcfierWinExecutablePath);
          }

          if (!File.Exists(bcfierWinExecutablePath))
          {
            throw new Exception($"The Bcfier.Win.exe path in the configuration is given as: \"{bcfierWinExecutablePath}\", but the file could not be found.");
          }

          return bcfierWinExecutablePath;
        }
      }
    }

    private static string GetConfigurationFilePath()
    {
      // The configuration file should always be adjacent to the Bcfier.dll assembly
      var currentFolder = GetCurrentDllDirectory();

      var configurationFilePath = Path.Combine(currentFolder, "Bcfier.Revit.Configuration.json");
      return configurationFilePath;
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