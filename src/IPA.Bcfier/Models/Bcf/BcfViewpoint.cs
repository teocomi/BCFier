using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfViewpoint
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public List<BcfViewpointClippingPlane> ClippingPlanes { get; set; } = new();

        [Required]
        public List<BcfViewpointLine> Lines { get; set; } = new();

        public string SnapshotBase64 { get; set; } = string.Empty;

        public BcfViewpointOrthogonalCamera? OrthogonalCamera { get; set; }

        public BcfViewpointPerspectiveCamera? PerspectiveCamera { get; set; }

        [Required]
        public BcfViewpointComponents ViewpointComponents { get; set; } = new();
    }
}
