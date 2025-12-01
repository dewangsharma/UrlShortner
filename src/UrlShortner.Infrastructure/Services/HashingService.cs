using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Settings;

namespace UrlShortner.Infrastructure.Services
{
    public class HashingService : IHashingService
    {
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly HashSettings _hashSettings;

        public HashingService(IOptions<HashSettings> hashSettingsOption)
        {
                _hashSettings = hashSettingsOption.Value;
        }

        public string GetHash(string plainText)
        {
            byte[] saltBytes = Encoding.ASCII.GetBytes(_hashSettings.Salt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(plainText),
                saltBytes,
                _hashSettings.Iterations,
                hashAlgorithm,
                _hashSettings.KeySize);

            return Convert.ToHexString(hash);
        }

        public bool Verify(string plainText, string hashed)
        {
            byte[] textBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] saltBytes = Encoding.ASCII.GetBytes(_hashSettings.Salt);

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(textBytes, saltBytes, _hashSettings.Iterations, hashAlgorithm, _hashSettings.KeySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashed));
        }
    }
}
