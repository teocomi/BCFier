using CefSharp.Handler;
using CefSharp;

namespace IPA.Bcfier.Revit
{
    public class PluginRequestHandler : RequestHandler
    {
        private readonly bool _proxyToLocalhost;

        public PluginRequestHandler(bool proxyToLocalhost)
        {
            _proxyToLocalhost = proxyToLocalhost;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return _proxyToLocalhost ? new PluginDebugRequestHandler()
                : new PluginResourceRequestHandler();
        }
    }
}
