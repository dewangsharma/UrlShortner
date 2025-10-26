namespace UrlShortner.Application.Models.Users
{
    public record UserDto
    {
        public int Id { get; set; }
        public required string FirstName { get; init; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}
