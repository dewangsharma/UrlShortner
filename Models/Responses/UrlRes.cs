namespace DataTypes.Responses
{
    public record UrlRes
    {
        public required int Id { get; set; }
        public required string Actual { get; set; }
        public required string Shortened { get; set; }

        public required UrlStatus Status { get; set; }
    }
}
