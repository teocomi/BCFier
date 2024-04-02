using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public abstract class BcfViewpointCameraBase
    {
        [Required]
        public double AspectRatio { get; set; }

        [Required]
        public BcfViewpointVector Direction { get; set; } = new();

        [Required]
        public BcfViewpointVector UpVector { get; set; } = new();

        [Required]
        public BcfViewpointPoint ViewPoint { get; set; } = new();
    }
}
