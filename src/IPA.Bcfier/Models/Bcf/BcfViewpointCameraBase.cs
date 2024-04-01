namespace IPA.Bcfier.Models.Bcf
{
    public abstract class BcfViewpointCameraBase
    {
        public double AspectRatio { get; set; }

        public BcfViewpointVector Direction { get; set; } = new();

        public BcfViewpointVector UpVector { get; set; } = new();

        public BcfViewpointPoint ViewPoint { get; set; } = new();
    }
}
