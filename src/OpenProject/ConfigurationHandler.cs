using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace OpenProject
{
  public static class ConfigurationHandler
  {
    public static bool ShouldEnableDevelopmentTools()
    {
      var configuration = ReadConfigurationFile();
      var setting = configuration["EnableDevelopmentTools"];
      if (setting == null)
      {
        return false;
      }

      return setting.Value<bool>();
    }

    public static void RemoveSavedInstance(string instanceUrl)
    {
      var configuration = ReadConfigurationFile();
      var existingValues = configuration["OpenProjectInstances"];
      if (existingValues != null)
      {
        var array = existingValues as JArray;
        var existing = array.FirstOrDefault(e => e.ToString() == instanceUrl);
        if (existing != null)
        {
          array.Remove(existing);
          SaveConfigurationFile(configuration);
        }
      }
    }

    public static List<string> LoadAllInstances()
    {
      var configuration = ReadConfigurationFile();
      var existingValues = configuration["OpenProjectInstances"];
      if (existingValues != null
          && existingValues is JArray array)
      {
        return existingValues.Select(i => i.ToString()).ToList();
      }

      return new List<string>();
    }

    public static void SaveSelectedInstance(string instanceUrl)
    {
      var configuration = ReadConfigurationFile();
      var existingValues = configuration["OpenProjectInstances"];
      if (existingValues == null)
      {
        configuration["OpenProjectInstances"] = new JArray();
        (configuration["OpenProjectInstances"] as JArray).Add(instanceUrl);
      }
      else
      {
        var array = existingValues as JArray;
        var existing = array.FirstOrDefault(e => e.ToString() == instanceUrl);
        if (existing != null)
        {
          array.Remove(existing);
        }
        array.AddFirst(instanceUrl);
      }

      SaveConfigurationFile(configuration);
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

    private static void SaveConfigurationFile(JObject configuration)
    {
      var json = configuration.ToString(Newtonsoft.Json.Formatting.Indented);
      var configurationFilePath = GetConfigurationFilePath();
      using (var fs = File.CreateText(configurationFilePath))
      {
        fs.Write(json);
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
