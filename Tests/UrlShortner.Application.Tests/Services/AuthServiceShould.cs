using Moq;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Models.Authentications;
using UrlShortner.Application.Repositories;
using UrlShortner.Application.Services;
using UrlShortner.Application.Tests.ClassFixtures;
using UrlShortner.Domain;

namespace UrlShortner.Application.Tests.Services
{
    public class AuthServiceShould : IClassFixture<AuthServiceFixture>
    {
        private readonly AuthService _authService;
        private readonly Mock<IEncryptionService> _mockEncryptionService;
        private readonly Mock<IHashingService> _mockHashingService;
        private readonly Mock<IUserCredentialRepository> _mockUserCredentialRepository;
        private readonly Mock<IUserTokenRepository> _mockUserTokenRepository;

        public AuthServiceShould(AuthServiceFixture authFixture)
        {
            _authService = authFixture.Sut;
            _mockEncryptionService = authFixture.MockEncryptionService;
            _mockHashingService = authFixture.MockHashingService;
            _mockUserCredentialRepository = authFixture.MockUserCredentialRepository;
            _mockUserTokenRepository = authFixture.MockUserTokenRepository;
        }

        [Fact]
        public async Task Success_LoginAsync()
        {
            // Arrange
            var loginDto = new LoginDto() { UserName = "user@example.com", Password = "Admin@123", IPAddress = "192.168.0.1" };

            var encryptedUsername = "encrypted-email-address";
            var hashedPassword = "hashed-password";
            var userCredential = new UserCredential
            {
                Id = 1,
                Username = encryptedUsername,
                Password = hashedPassword,
                UserId = 1001,
                User = new User() { Id = 1001, FirstName = "FirstName", LastName = "LastName", Email = encryptedUsername, UserRoles = new List<UserRole>() { new UserRole { Id = 111, Role = new Role() { Id = 777, Name = "Corporate" } } } }
            };

            _mockEncryptionService.Setup(x => x.EncryptAsync(It.IsAny<string>())).Returns(async () => encryptedUsername);
            _mockUserCredentialRepository.Setup(url => url.FindAsync(user => user.Username == encryptedUsername)).ReturnsAsync(() => userCredential);
            _mockHashingService.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _mockUserTokenRepository.Setup(x => x.AddAsync(It.IsAny<UserToken>()));

            // Act
            var response = await _authService.LoginAsync(loginDto, new CancellationToken());

            // Assert
            Assert.NotNull(response);
            Assert.IsType<LoginResponseDto>(response);
        }
    }
}
