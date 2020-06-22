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
      // The configuration file should always be adjacent to the Bcfier.dll assembly
      var currentAssemblyPathUri = Assembly.GetExecutingAssembly().CodeBase;
      var currentAssemblyPath = Uri.UnescapeDataString(new Uri(currentAssemblyPathUri).AbsolutePath)
        // '/' comes from the uri, we need it to be '\' for the path
        .Replace("/", "\\");
      var currentFolder = Path.GetDirectoryName(currentAssemblyPath);
      var configurationFilePath = Path.Combine(currentFolder, "OpenProject.Configuration.json");
      return configurationFilePath;
    }
  }
}
