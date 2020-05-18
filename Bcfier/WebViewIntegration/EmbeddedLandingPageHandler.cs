using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bcfier.WebViewIntegration
{
  public class EmbeddedLandingPageHandler
  {
    public static string GetEmbeddedLandingPageIndexUrl()
    {
      var indexFilePath = GetIndexFilePath();

      return $"file:///{indexFilePath}";
    }

    private static string GetIndexFilePath()
    {
      // The unpacked Html should be adjacent to this Dll
      var currentAssemblyPathUri = Assembly.GetExecutingAssembly().CodeBase;
      var currentAssemblyPath = Uri.UnescapeDataString(new Uri(currentAssemblyPathUri).AbsolutePath)
        // '/' comes from the uri, we need it to be '\' for the path
        .Replace("/", "\\");
      var currentFolder = Path.GetDirectoryName(currentAssemblyPath);
      // We're versioning the folder so as to not have to do a direct file comparison of the contents
      // in case an earlier version was aready present
      var landingPageFolder = Path.Combine(currentFolder, "LandingPage", VersionsService.Version);
      if (!Directory.Exists(landingPageFolder))
      {
        ExtractEmbeddedLandingPageToFolder(landingPageFolder);
      }

      return Path.Combine(landingPageFolder, "index.html");
    }

    private static void ExtractEmbeddedLandingPageToFolder(string landingPageFolder)
    {
      var tempPath = Path.GetTempFileName();
      using (var resourceStream = GetEmbeddedResourceZipStream())
      {
        using (var fs = System.IO.File.Create(tempPath))
        {
          resourceStream.CopyTo(fs);
        }
      }

      ZipFile.ExtractToDirectory(tempPath, landingPageFolder);
    }

    private static Stream GetEmbeddedResourceZipStream()
    {
      var assembly = Assembly.GetExecutingAssembly();
      var resourceName = "Bcfier.WebViewIntegration.LandingPage.LandingPage.zip";
      return assembly.GetManifestResourceStream(resourceName);
    }
  }
}
