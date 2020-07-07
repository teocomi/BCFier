using OpenProject.Data.Utils;
using CefSharp;
using CefSharp.Wpf;
using System.Reflection;

namespace OpenProject.WebViewIntegration
{
  /// <summary>
  /// This class is used to initialize the embedded browser view.
  /// </summary>
  public static class CefBrowserInitializer
  {
    /// <summary>
    /// This method must be called from the main UI thread, before any instances of the embedded browser view
    /// are created anywhere in the application. This configures the global settings for the embedded browser view.
    /// </summary>
    public static void InitializeCefBrowser()
    {
      var settings = new CefSettings();

      // We're using a custom user agent for the embedded web browser view to ensure that the OpenProject web service
      // is aware that we're accessing from the BCFier app.
      var chromiumVersion = Cef.ChromiumVersion;
      var userAgent = $"Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{chromiumVersion} Safari/537.36" +
        $" /OpenProjectRevitPlugin {VersionsService.Version}";
      settings.UserAgent = userAgent;

      // To enable caching, e.g. of assets and cookies, we're using a temp data folder
      settings.CachePath = System.IO.Path.Combine(UserSettings.BCFierAppDataFolder, "BrowserCache");
      // Additionally, we're persisting session cookies to ensure logins are persistent throughout
      // multiple sessions.
      settings.PersistSessionCookies = true;

      var assemblyLocation = Assembly.GetExecutingAssembly().Location;
      var assemblyPath = System.IO.Path.GetDirectoryName(assemblyLocation);
      var pathSubprocess = System.IO.Path.Combine(assemblyPath, "CefSharp.BrowserSubprocess.exe");
      settings.BrowserSubprocessPath = pathSubprocess;

      Cef.Initialize(settings);
    }
  }
}
