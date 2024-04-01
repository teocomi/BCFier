using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpointComponentVisibility
    {
        public bool DefaultVisibility { get; set; } = true;

        public List<BcfViewpointComponent> Exceptions { get; set; } = new();
    }
}
