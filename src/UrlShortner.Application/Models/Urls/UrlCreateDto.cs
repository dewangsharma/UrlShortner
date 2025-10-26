namespace UrlShortner.Application.Models.Urls
{
    public record UrlCreateDto
    {
        public required string Actual { get; set; }
        // public required string Shortened { get; set; }

        public int UserId { get; set; }
    }
}
