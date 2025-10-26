namespace UrlShortner.Application.Models.Authentications
{
    public record LoginDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string IPAddress { get; set; }
    }
}
