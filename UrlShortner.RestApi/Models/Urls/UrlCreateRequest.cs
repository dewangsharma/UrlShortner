namespace UrlShortner.RestApi.Models.Urls
{
    public record UrlCreateRequest
    {
        public required string Actual { get; set; }
        // public required string Shortened { get; set; }
    }
}
