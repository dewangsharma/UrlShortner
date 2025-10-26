namespace UrlShortner.RestApi.Models.Urls
{
    public record UrlResponse
    {
        public required int Id { get; set; }
        public required string Actual { get; set; }
        public required string Shortened { get; set; }

        public required UrlStatusRequest Status { get; set; }
    }
    
}
