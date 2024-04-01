using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfProjectExtensions
    {
        public List<string> Users { get; set; } = new();
        public List<string> TopicLabels { get; set; } = new();
        public List<string> TopicTypes { get; set; } = new();
        public List<string> Priorities { get; set; } = new();
        public List<string> SnippetTypes { get; set; } = new();
        public List<string> TopicStatuses { get; set; } = new();
    }
}
