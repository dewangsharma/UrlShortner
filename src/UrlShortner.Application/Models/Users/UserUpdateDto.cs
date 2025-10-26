namespace UrlShortner.Application.Models.Users
{
    public record UserUpdateDto
    {
        public required string FirstName { get; init; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
    }
}
