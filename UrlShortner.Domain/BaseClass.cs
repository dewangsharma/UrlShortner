using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record class BaseClass
    {
        [Required]
        public Guid UuId { get; set; } = Guid.NewGuid();
    }
}
