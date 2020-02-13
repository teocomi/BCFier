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
      // The configuration file should always be adjacent to the Bcfier.Revit.dll assembly
      var currentAssemblyPath = Assembly.GetExecutingAssembly().CodeBase;
      using (var fs = System.IO.File.OpenRead(currentAssemblyPath))
      {
        using (var sr = new StreamReader(fs))
        {
          var json = sr.ReadToEnd();
          var jObject = JObject.Parse(json);
          var bcfierWinExecutablePath = jObject["BcfierWinExecutablePath"].ToString();
          if (!File.Exists(bcfierWinExecutablePath))
          {
            throw new Exception($"The Bcfier.Win.exe path in the configuration is given as: \"{bcfierWinExecutablePath}\", but the file could not be found.");
          }

          return bcfierWinExecutablePath;
        }
      }
    }
  }
}