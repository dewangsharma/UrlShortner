namespace UrlShortner.Application.Models.Authentications
{
    public record EncryptionResponse
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
