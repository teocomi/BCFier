using System;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfDocumentReference
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Url { get; set; } = string.Empty;

        public string DocumentId { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
