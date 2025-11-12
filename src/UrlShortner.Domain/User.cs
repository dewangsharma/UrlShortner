using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record User: DateTimeStamp
    {
        [Key]
        public int Id { get; set; }
        public required string FirstName { get; init; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public UserCredential UserCredential { get; set; }
        public ICollection<Url> Urls { get; set; }

        public ICollection<UserToken> UserTokens { get; set; }

        public ICollection<UserUrl> UserUrls { get; set; } = new List<UserUrl>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
