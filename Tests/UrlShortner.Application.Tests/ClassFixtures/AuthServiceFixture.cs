using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Repositories;
using UrlShortner.Application.Services;
using UrlShortner.Application.Settings;

namespace UrlShortner.Application.Tests.ClassFixtures
{
    public class AuthServiceFixture: IDisposable
    {
        internal AuthService Sut { get; init; }
        internal Mock<ILogger<AuthService>> Mocklogger { get; init; }
        internal Mock<IOptions<JwtSettings>> MockJwtSettingsOption { get; init; }
        internal Mock<IEncryptionService> MockEncryptionService{ get; init; }
        internal Mock<IHashingService> MockHashingService { get; init; }
        internal Mock<IUserCredentialRepository> MockUserCredentialRepository { get; init; }
        internal Mock<IUserTokenRepository> MockUserTokenRepository { get; init; }

        public AuthServiceFixture()
        {
            Mocklogger = new Mock<ILogger<AuthService>>();
            MockJwtSettingsOption = new Mock<IOptions<JwtSettings>>();
            MockEncryptionService = new Mock<IEncryptionService>();
            MockHashingService = new Mock<IHashingService>();
            MockUserCredentialRepository = new Mock<IUserCredentialRepository>();
            MockUserTokenRepository = new Mock<IUserTokenRepository>();

            var jwtSetting = new JwtSettings() { SecretKey = "dsdsdsdsdsdsdsdsdsDS123456789DSdsds1/DS1220=", Issuer = "DewangLocalApp", Audience = "*", TokenExpiresMinutes = 15 };

            MockJwtSettingsOption.Setup(x => x.Value).Returns(jwtSetting);
            Sut = new AuthService(Mocklogger.Object, MockJwtSettingsOption.Object, MockEncryptionService.Object, MockHashingService.Object, MockUserCredentialRepository.Object, MockUserTokenRepository.Object);
        }

        public void Dispose()
        {
        }
    }
}
