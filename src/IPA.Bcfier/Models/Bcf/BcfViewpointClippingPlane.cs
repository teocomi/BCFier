using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointClippingPlane
    {
        [Required]
        public BcfViewpointPoint Location { get; set; } = new();

        [Required]
        public BcfViewpointVector Direction { get; set; } = new();
    }
}
