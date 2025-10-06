using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record class BaseClass
    {
        [Key]
        public int Id { get; set; }

        [Key]
        public Guid UuId { get; set; }
    }
}
