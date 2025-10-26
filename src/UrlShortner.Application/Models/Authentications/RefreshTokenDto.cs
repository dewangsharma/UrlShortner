namespace UrlShortner.Application.Models.Authentications
{
    public record RefreshTokenDto
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public required string IPAddress { get; set; }
    }
}
