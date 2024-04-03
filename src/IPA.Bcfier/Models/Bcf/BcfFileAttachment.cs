using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Bcf
{
    public class BcfFileAttachment
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Base64Data { get; set; } = string.Empty;
    }
}
