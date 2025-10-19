namespace UrlShortner.Domain
{
    public record class Url: DateTimeStamp
    {
        public int UserId { get; set; }
        public required string Actual { get; set; }
        public required string Shortened { get; set; }
        public required UrlStatus Status { get; set; }

        public User? User { get; set; }
    }

    public enum UrlStatus 
    {
        Inactive = 0,
        Active = 1
    }
}
