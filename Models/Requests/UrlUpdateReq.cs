namespace DataTypes.Requests
{
    public record UrlUpdateReq
    {
        public required int Id { get; set; }
        public required UrlStatus Status { get; set; }
    }
}
