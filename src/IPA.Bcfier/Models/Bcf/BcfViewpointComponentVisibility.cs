using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponentVisibility
    {
        [Required]
        public bool DefaultVisibility { get; set; } = true;

        [Required]
        public List<BcfViewpointComponent> Exceptions { get; set; } = new();
    }
}
