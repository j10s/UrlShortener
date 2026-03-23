using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UpdateLinkRequest
    {
        [Required]
        [Url]
        public string TargetUri { get; set; }
    }
}
