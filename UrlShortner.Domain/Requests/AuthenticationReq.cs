namespace DataTypes.Requests
{
    public record AuthenticationReq
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
