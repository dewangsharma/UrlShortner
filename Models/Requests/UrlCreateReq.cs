namespace DataTypes.Requests
{
    public record UrlCreateReq
    {
        public required string Actual { get; set; }
        public required string Shortened { get; set; }
    }
}
