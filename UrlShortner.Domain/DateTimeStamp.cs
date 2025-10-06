using UrlShortner.Domain;

namespace DataTypes
{
    public record DateTimeStamp: BaseClass
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
