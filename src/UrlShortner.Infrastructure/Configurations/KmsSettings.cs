namespace UrlShortner.Infrastructure.Configurations
{
    public record class KmsSettings
    {
        public string KeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string ServiceURL { get; set; }
        public string Region { get; set; }
        public string Profile { get; set; }
    }
}
