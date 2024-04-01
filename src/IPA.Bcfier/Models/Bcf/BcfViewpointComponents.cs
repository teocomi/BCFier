using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponents
    {
        public List<BcfViewpointComponentColoring> Coloring { get; set; } = new();

        public List<BcfViewpointComponent> SelectedComponents { get; set; } = new();

        public BcfViewpointComponentVisibility Visibility { get; set; } = new();
    }
}
