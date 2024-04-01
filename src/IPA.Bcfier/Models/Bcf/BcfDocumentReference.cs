using System;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfDocumentReference
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Url { get; set; } = string.Empty;

        public string DocumentId { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
