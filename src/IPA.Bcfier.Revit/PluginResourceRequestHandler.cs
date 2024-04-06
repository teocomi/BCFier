using CefSharp.Handler;
using CefSharp;
using System.Reflection;

namespace IPA.Bcfier.Revit
{
    public class PluginResourceRequestHandler : ResourceRequestHandler
    {
        protected override IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            if (request.Url.EndsWith("api/frontend-config/config.js"))
            {
                var frontendConfig = @"(async function() {
window.ipaBcfierFrontendConfig = {""isInElectronMode"": false};
await window[""cefSharp""].bindObjectAsync(""bcfierJavascriptBridge"");
})();";
                return ResourceHandler.FromString(frontendConfig, mimeType: "text/javascript");
            }

            var fileName = Path.GetFileName(request.Url.TrimEnd('/'));
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"IPA.Bcfier.Revit.Resources.Browser.{fileName}");

            if (stream == null)
            {
                var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                    .Where(r => r.EndsWith(fileName))
                    .ToList();
                if (resourceNames.Count == 1)
                {
                    stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceNames[0]);
                }
            }

            if (stream != null)
            {
                if (fileName.EndsWith(".js"))
                {
                    return ResourceHandler.FromStream(stream, "text/javascript");
                }
                else if (fileName.EndsWith(".css"))
                {
                    return ResourceHandler.FromStream(stream, "text/css");
                }
                else if (fileName.EndsWith(".woff2"))
                {
                    return ResourceHandler.FromStream(stream, "font/woff2");
                }
                else
                {
                    return ResourceHandler.FromStream(stream);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
