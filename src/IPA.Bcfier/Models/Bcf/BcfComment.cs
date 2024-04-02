using System;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfComment
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public DateTime? CreationDate { get; set; }

        public string ModifiedBy { get; set; } = string.Empty;

        public DateTime? ModifiedDate { get; set; }

        public Guid? ViewpointId { get; set; }
    }
}
