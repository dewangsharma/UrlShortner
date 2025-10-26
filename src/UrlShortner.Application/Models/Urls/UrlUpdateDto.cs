using UrlShortner.Domain;

namespace UrlShortner.Application.Models.Urls
{
    public record UrlUpdateDto
    {
        public required int Id { get; set; }
        public required UrlStatusDto Status { get; set; }
    }
}
