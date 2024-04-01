using System;
using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfTopic
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<BcfTopicFile> Files { get; set; } = new();

        public List<BcfViewpoint> Viewpoints { get; set; } = new();

        public List<BcfDocumentReference> DocumentReferences { get; set; } = new();

        public List<BcfComment> Comments { get; set; } = new();

        public string AssignedTo { get; set; } = string.Empty;

        public string CreationAuthor { get; set; } = string.Empty;

        public DateTime? CreationDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ModifiedAuthor { get; set; } = string.Empty;

        public DateTime? ModifiedDate { get; set; }

        public string ServerAssignedId { get; set; } = string.Empty;

        public string TopicStatus { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Stage { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string TopicType { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public int Index { get; set; }

        public List<string> Labels { get; set; } = new();

        public List<Guid> RelatedTopicIds { get; set; } = new();

        public List<string> ReferenceLinks { get; set; } = new();
    }
}
