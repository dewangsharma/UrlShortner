namespace UrlShortner.RestApi.Models.Users
{
    public record UserUpdateRequest
    {
        public required string FirstName { get; init; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; }
    }
}
