using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Microsoft.Extensions.Options;
using System.Text;
using UrlShortner.Application.Interfaces;
using UrlShortner.Infrastructure.Configurations;

namespace UrlShortner.Infrastructure.Services
{
    public class AwsKmsEncryptionService : IEncryptionService
    {
        private readonly IAmazonKeyManagementService _awsKmsService;
        private readonly KmsSettings _settings;

        public AwsKmsEncryptionService(IAmazonKeyManagementService awsKmsService, IOptions<KmsSettings> settings)
        {
            _awsKmsService = awsKmsService;
            _settings = settings.Value;
        }

        public async Task<string> EncryptAsync(string plainText)
        {
            var request = new EncryptRequest
            {
                KeyId = _settings.KeyId,
                Plaintext = new MemoryStream(Encoding.UTF8.GetBytes(plainText))
            };

            var response = await _awsKmsService.EncryptAsync(request);

            return Convert.ToBase64String(response.CiphertextBlob.ToArray());
        }

        public async Task<string> DecryptAsync(string cipherText)
        {
            var request = new DecryptRequest
            {
                CiphertextBlob = new MemoryStream(Convert.FromBase64String(cipherText))
            };

            var response = await _awsKmsService.DecryptAsync(request);

            return Encoding.UTF8.GetString(response.Plaintext.ToArray());
        }
    }
}
