namespace UrlShortner.RestApi.Models.Urls
{
    public record UrlUpdateRequest
    {
        public required int Id { get; set; }
        public required UrlStatusRequest Status { get; set; }
    }
}
