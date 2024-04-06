using Autodesk.Revit.DB;
using CefSharp;

namespace IPA.Bcfier.Revit.Services
{
    public class ViewContinuationInstructions
    {
        public Action? ViewContinuation { get; set; }

        public ElementId? ViewId { get; set; }

        public IJavascriptCallback JavascriptCallback { get; set; }
    }
}
