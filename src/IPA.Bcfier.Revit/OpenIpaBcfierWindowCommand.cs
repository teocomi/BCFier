using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CefSharp;
using CefSharp.Wpf;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IPA.Bcfier.Revit
{
    [Transaction(TransactionMode.Manual)]
    public class OpenIpaBcfierWindowCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var browser = new ChromiumWebBrowser();
#if DEBUG_BUILD
            var proxyToLocalhost = true;
#else
            var proxyToLocalhost = false;
#endif
            browser.JavascriptObjectRepository.Register("bcfierJavascriptBridge", new BcfierJavascriptBridge(), true);
            browser.RequestHandler = new PluginRequestHandler(proxyToLocalhost);
            browser.IsBrowserInitializedChanged += (s, e) =>
            {
                if (browser.IsBrowserInitialized)
                {
                    browser.ShowDevTools();
                }
            };


#if DEBUG_BUILD
            browser.Load("http://localhost:4200");
#else
            browser.Load("index.html");
#endif

            var window = new Window();
            window.Content = browser;
            using var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("IPA.Bcfier.Revit.Resources.Browser.favicon.ico");
            if (iconStream != null)
            {
                window.Icon = BitmapFrame.Create(iconStream);
            }

            window.Show();

            return Result.Succeeded;
        }
    }
}
