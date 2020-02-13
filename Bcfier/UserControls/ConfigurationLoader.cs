using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Bcfier.UserControls
{
  public static class ConfigurationLoader
  {
    public static string LoadBcfierBrowserInitialAddressOrNull()
    {
      var configurationFilePath = GetConfigurationFilePath();
      using (var fs = File.OpenRead(configurationFilePath))
      {
        using (var sr = new StreamReader(fs))
        {
          var json = sr.ReadToEnd();
          var jObject = JObject.Parse(json);
          var initialBrowserAddress = jObject["InitialBrowserAddress"]?.ToString();

          return initialBrowserAddress;
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
      var configurationFilePath = Path.Combine(currentFolder, "Bcfier.Configuration.json");
      return configurationFilePath;
    }
  }
}
