using System.ComponentModel.DataAnnotations;

namespace DataTypes
{
    public record UserToken: DateTimeStamp
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken{ get; set; }
        public DateTime RefreshTokenExpired { get; set; }
        public string IPAddress { get; set; }

        public User? User { get; set; }
    }

    public enum UserTokenStatus
    {
        InActive = 0,
        Active = 1
    }
}
