using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponentColoring
    {
        [Required]
        public string Color { get; set; } = string.Empty;

        [Required]
        public List<BcfViewpointComponent> Components { get; set; } = new();
    }
}
