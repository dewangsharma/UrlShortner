namespace DataTypes
{
    public record UserCredential : DateTimeStamp
    {

        public required string Username { get; set; }

        public required string Password { get; set; }

        public User? User { get; set; }

        //public string UsernameSalt { get; set; }
        //public string PasswordSalt { get; set; }
    }
}
