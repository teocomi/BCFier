using System;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfTopicFile
    {
        [Required]
        public string FileName { get; set; } = string.Empty;

        public string ReferenceLink { get; set; } = string.Empty;

        public string IfcProjectId { get; set; } = string.Empty;

        public string IfcSpatialStructureElementId { get; set; } = string.Empty;

        public DateTime? Date { get; set; }
    }
}
