using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointLine
    {
        [Required]
        public BcfViewpointPoint StartPoint { get; set; } = new();

        [Required]
        public BcfViewpointPoint EndPoint { get; set; } = new();
    }
}
