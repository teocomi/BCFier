using CefSharp.Handler;
using CefSharp;
using System.Net.Http;

namespace IPA.Bcfier.Revit
{
    public class PluginDebugRequestHandler : ResourceRequestHandler
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

            return null;
        }
    }
}
