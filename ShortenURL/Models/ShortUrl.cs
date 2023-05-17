using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ShortenURL.Models
{
    public class ShortUrl
    {
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Original URL")]
        public string Url { get; set; }

        [Display(Name = "Short URL")]
        public string? ShorUrl { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Description")]
        public string Description{ get; set; }

        public ShortUrl() { }
    }
}
