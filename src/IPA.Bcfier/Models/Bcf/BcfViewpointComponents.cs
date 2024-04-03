using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponents
    {
        [Required]
        public List<BcfViewpointComponentColoring> Coloring { get; set; } = new();

        [Required]
        public List<BcfViewpointComponent> SelectedComponents { get; set; } = new();

        [Required]
        public BcfViewpointComponentVisibility Visibility { get; set; } = new();
    }
}
