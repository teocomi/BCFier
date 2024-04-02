using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointPerspectiveCamera : BcfViewpointCameraBase
    {
        [Required]
        public double FieldOfView { get; set; }
    }
}
