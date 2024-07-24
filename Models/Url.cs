using System.ComponentModel.DataAnnotations;

namespace DataTypes
{
    public record class Url: DateTimeStamp
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Actual { get; set; }
        public required string Shortened { get; set; }
        public required UrlStatus Status { get; set; }

        public User? User { get; set; }
    }

    public enum UrlStatus 
    {
        Inactive = 1,
        Active = 1
    }
}
