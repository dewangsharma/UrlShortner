namespace DataTypes.Requests
{
    public record RefreshTokenReq
    {
        public required string Token { get; set; }
        public required string RefreshToken{ get; set; }
    }
}
