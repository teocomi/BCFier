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
            EnsureDependentAssembliesAreLoaded();

            var browser = new ChromiumWebBrowser();
#if DEBUG_BUILD
            var proxyToLocalhost = true;
#else
            var proxyToLocalhost = false;
#endif

            var taskQueueHandler = new RevitTaskQueueHandler();
            var bcfierJavascriptBridge = new BcfierJavascriptBridge(taskQueueHandler);
            browser.JavascriptObjectRepository.Register("bcfierJavascriptBridge", bcfierJavascriptBridge, true);
            browser.RequestHandler = new PluginRequestHandler(proxyToLocalhost);
#if DEBUG_BUILD
            browser.IsBrowserInitializedChanged += (s, e) =>
            {
                if (browser.IsBrowserInitialized)
                {
                    browser.ShowDevTools();
                }
            };
#endif

#if DEBUG_BUILD
            browser.Load("http://localhost:4200");
#else
            browser.Load("index.html");
#endif

            commandData.Application.Idling += taskQueueHandler.OnIdling;

            var window = new Window();
            window.Content = browser;
            using var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("IPA.Bcfier.Revit.Resources.Browser.favicon.ico");
            if (iconStream != null)
            {
                window.Icon = BitmapFrame.Create(iconStream);
            }

            window.Closed += (s, e) =>
            {
                taskQueueHandler.UnregisterEventHandler();
                browser.Dispose();
            };

            window.Show();

            return Result.Succeeded;
        }

        private void EnsureDependentAssembliesAreLoaded()
        {
            // I didn't find out why this is required, but apparently Revit
            // does something to resolve assemblies, and this seems to fail
            // when the assemblies are not directly loaded due to execution in the
            // initial command but only later, like in our case when an event from
            // the browser is triggering some action
            typeof(IPA.Bcfier.Models.Bcf.BcfComment).ToString();
            typeof(Dangl.BCF.APIObjects.V21.Auth_GET).ToString();
            typeof(IPA.Bcfier.Ipc.IpcHandler).ToString();
            typeof(DecimalMath.DecimalEx).ToString();
        }
    }
}
