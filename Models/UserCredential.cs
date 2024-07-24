using System.ComponentModel.DataAnnotations;

namespace DataTypes
{
    public record UserCredential : DateTimeStamp
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User? User { get; set; }

        //public string UsernameSalt { get; set; }
        //public string PasswordSalt { get; set; }
    }
}
