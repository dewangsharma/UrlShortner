using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record class Url: DateTimeStamp
    {
        [Key]
        public int Id { get; set; }
        public required string Actual { get; set; }
        public required string Shortened { get; set; }
        public required UrlStatus Status { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<UserUrl> UserUrls { get; set; } = new List<UserUrl>();
    }

    public enum UrlStatus 
    {
        Inactive = 0,
        Active = 1
    }
}
