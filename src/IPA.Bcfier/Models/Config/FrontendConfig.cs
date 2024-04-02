using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Config
{
    public class FrontendConfig
    {
        [Required]
        public bool IsInElectronMode { get; set; } = false;
    }
}
