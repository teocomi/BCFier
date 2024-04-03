using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponent
    {
        public string OriginatingSystem { get; set; } = string.Empty;

        public string AuthoringToolId { get; set; } = string.Empty;

        [Required]
        public string IfcGuid { get; set; } = string.Empty;
    }
}
