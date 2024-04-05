using CefSharp;
using IPA.Bcfier.Models.Bcf;

namespace IPA.Bcfier.Revit.Models
{
    public class SaveBcfFileQueueItem
    {
        public IJavascriptCallback? Callback { get; set; }

        public BcfFile? BcfFile { get; set; }
    }
}
