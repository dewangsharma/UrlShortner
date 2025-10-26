namespace UrlShortner.Application.Models.Authentications
{
    public record LoginResponseDto
    {
        public required string TokenType { get; init; }
        public required string Token { get; init; }
        public required int ExpiresIn { get; init; }
        public required string RefreshToken { get; set; }

        // public required DateTime Expiration { get; init; }
        //public required int Id { get; init; }
        //public required string FirstName { get; init; }
        //public required string LastName { get; init; }
        //public required string Email { get; init; }
    }
}
