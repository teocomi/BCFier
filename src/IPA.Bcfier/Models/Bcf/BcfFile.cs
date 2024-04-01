using System.Collections.Generic;

namespace IPA.Bcfier.Models.Bcf
{

    public class BcfFile
    {
        public string FileName { get; set; } = string.Empty;

        public BcfProject Project { get; set; } = new();

        public List<BcfFileAttachment> FileAttachments { get; set; } = new();

        public BcfProjectExtensions ProjectExtensions { get; set; } = new();

        public List<BcfTopic> Topics { get; set; } = new();
    }
}
