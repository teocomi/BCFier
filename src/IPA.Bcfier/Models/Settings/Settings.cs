using System.ComponentModel.DataAnnotations;

namespace IPA.Bcfier.Models.Settings
{
    public class Settings
    {
        [Required]
        public string Username { get; set; } = string.Empty;
    }
}
