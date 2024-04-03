using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointOrthogonalCamera : BcfViewpointCameraBase
    {
        [Required]
        public double ViewToWorldScale { get; set; }
    }
}
