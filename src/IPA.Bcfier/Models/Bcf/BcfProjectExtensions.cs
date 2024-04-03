using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfProjectExtensions
    {
        [Required]
        public List<string> Users { get; set; } = new();

        [Required]
        public List<string> TopicLabels { get; set; } = new();

        [Required]
        public List<string> TopicTypes { get; set; } = new();

        [Required]
        public List<string> Priorities { get; set; } = new();

        [Required]
        public List<string> SnippetTypes { get; set; } = new();

        [Required]
        public List<string> TopicStatuses { get; set; } = new();
    }
}
