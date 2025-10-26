namespace UrlShortner.RestApi.Models.Authentications
{
    public record RefreshTokenRequest
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
