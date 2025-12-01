namespace UrlShortner.Application.Interfaces
{
    public interface IEncryptionService
    {
        Task<string> EncryptAsync(string plainText);
        Task<string> DecryptAsync(string cipherText);
    }
}
