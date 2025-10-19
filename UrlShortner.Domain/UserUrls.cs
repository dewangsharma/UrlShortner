namespace UrlShortner.Domain
{
    public record UserUrl: DateTimeStamp
    {
        public int UserId { get; set; }
        public int UrlId { get; set; }

        public User? User{ get; set; }
        public Url? Url{ get; set; }
    }
}
