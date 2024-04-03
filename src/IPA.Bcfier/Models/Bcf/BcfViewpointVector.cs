using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointVector
    {
        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        [Required]
        public double Z { get; set; }
    }
}
