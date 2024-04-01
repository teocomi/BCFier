using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponentColoring
    {
        public string Color { get; set; } = string.Empty;

        public List<BcfViewpointComponent> Components { get; set; } = new();
    }
}
