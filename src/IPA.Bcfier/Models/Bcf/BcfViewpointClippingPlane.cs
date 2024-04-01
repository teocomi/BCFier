namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointClippingPlane
    {
        public BcfViewpointPoint Location { get; set; } = new();

        public BcfViewpointVector Direction { get; set; } = new();
    }
}
