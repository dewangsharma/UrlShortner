namespace UrlShortner.Application.Settings
{
    public record JwtSettings
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string SecretKey { get; init; }
        public int KeySize { get; init; }
        public int Iterations { get; init; }
        public int TokenExpiresMinutes { get; init; }
    }
}
