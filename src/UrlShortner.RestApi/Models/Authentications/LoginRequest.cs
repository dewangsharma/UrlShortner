namespace UrlShortner.RestApi.Models.Authentications
{
    public record LoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}