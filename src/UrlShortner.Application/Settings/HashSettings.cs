namespace UrlShortner.Application.Settings
{
    public record class HashSettings
    {
        public int KeySize { get; init; }
        public int Iterations { get; init; }
        public string Salt { get; init; }
    }
}
