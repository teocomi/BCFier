using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace OpenProject
{
  public static class ConfigurationHandler
  {
    public static string LoadBcfierBrowserInitialAddressOrNull()
    {
      var configuration = ReadConfigurationFile();
      var initialBrowserAddress = configuration["InitialBrowserAddress"]?.ToString();
      return initialBrowserAddress;
    }

    public static void SaveInitialBrowserAddress(string browserAddress)
    {
      var configuration = ReadConfigurationFile();
      configuration["InitialBrowserAddress"] = browserAddress;
      var json = configuration.ToString(Newtonsoft.Json.Formatting.Indented);
      var configurationFilePath = GetConfigurationFilePath();
      using (var fs = File.CreateText(configurationFilePath))
      {
        fs.Write(json);
      }
    }

    private static JObject ReadConfigurationFile()
    {
      var configurationFilePath = GetConfigurationFilePath();
      using (var fs = File.OpenRead(configurationFilePath))
      {
        using (var sr = new StreamReader(fs))
        {
          var json = sr.ReadToEnd();
          var jObject = JObject.Parse(json);
          return jObject;
        }
      }
    }

    private static string GetConfigurationFilePath()
    {
      var configPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "OpenProject.Revit",
        "OpenProject.Configuration.json");

      if (!File.Exists(configPath))
      {
        // If the file doesn't yet exist, the default one is created
        using (var configStream = typeof(ConfigurationHandler).Assembly.GetManifestResourceStream("OpenProject.OpenProject.Configuration.json"))
        {
          var configDirName = Path.GetDirectoryName(configPath);
          if (!Directory.Exists(configDirName))
          {
            Directory.CreateDirectory(configDirName);
          }

          using (var fs = File.Create(configPath))
          {
            configStream.CopyTo(fs);
          }
        }
      }

      return configPath;
    }
  }
}
