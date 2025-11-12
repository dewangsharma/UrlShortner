using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record Role : DateTimeStamp
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
