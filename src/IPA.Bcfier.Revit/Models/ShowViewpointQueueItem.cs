using IPA.Bcfier.Models.Bcf;

namespace IPA.Bcfier.Revit.Models
{
    public class ShowViewpointQueueItem
    {
        public Func<Task>? Callback { get; set; }

        public BcfViewpoint? Viewpoint { get; set; }
    }
}
