using UrlShortner.Domain;

namespace UrlShortner.Application.Models.Urls
{
    public record UrlDto
    {
        public required int Id { get; set; }
        public required string Actual { get; set; }
        public required string Shortened { get; set; }

        public required UrlStatusDto Status { get; set; }
    }
}
