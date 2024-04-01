using System;
using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpoint
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<BcfViewpointClippingPlane> ClippingPlanes { get; set; } = new();

        public List<BcfViewpointLine> Lines { get; set; } = new();

        public string SnapshotBase64 { get; set; } = string.Empty;

        public BcfViewpointOrthogonalCamera? OrthogonalCamera { get; set; }

        public BcfViewpointPerspectiveCamera? PerspectiveCamera { get; set; }

        public BcfViewpointComponents ViewpointComponents { get; set; } = new();
    }
}
