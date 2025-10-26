using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record UserUrl: DateTimeStamp
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int UrlId { get; set; }

        public User? User{ get; set; }
        public Url? Url{ get; set; }
    }
}
