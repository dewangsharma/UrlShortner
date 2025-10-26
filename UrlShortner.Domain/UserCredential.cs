using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public record UserCredential: DateTimeStamp
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }

        public required string Password { get; set; }

        // ForeignKey
        public int UserId { get; set; }

        public User? User { get; set; }

        //public string UsernameSalt { get; set; }
        //public string PasswordSalt { get; set; }
    }
}
