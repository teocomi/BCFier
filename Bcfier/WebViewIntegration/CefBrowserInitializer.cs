using Bcfier.Data.Utils;
using CefSharp;
using CefSharp.Wpf;

namespace Bcfier.WebViewIntegration
{
  // TODO RENAME EVERYTHING FROM BCFIER TO OpenProjectRevitPlugin -> WHEN NAME IS FINALIZED

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
        " /OpenProjectRevitPlugin"; // TODO BCFier version -> implement GitVersion to automatically version BCFier based on the Git history
      settings.UserAgent = userAgent;

      // To enable caching, e.g. of assets and cookies, we're using a temp data folder
      settings.CachePath = System.IO.Path.Combine(UserSettings.BCFierAppDataFolder, "BrowserCache");
      // Additionally, we're persisting session cookies to ensure logins are persistent throughout
      // multiple sessions.
      settings.PersistSessionCookies = true;

      Cef.Initialize(settings);
    }
  }
}
