namespace DataTypes.Responses
{
    public record UserRes
    {
        public int Id { get; set; }
        public required string FirstName { get; init; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}
