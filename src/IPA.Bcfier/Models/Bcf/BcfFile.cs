using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfFile
    {
        [Required]
        public string FileName { get; set; } = string.Empty;

        public BcfProject Project { get; set; } = new();

        public List<BcfFileAttachment> FileAttachments { get; set; } = new();

        public BcfProjectExtensions ProjectExtensions { get; set; } = new();

        [Required]
        public List<BcfTopic> Topics { get; set; } = new();
    }
}
