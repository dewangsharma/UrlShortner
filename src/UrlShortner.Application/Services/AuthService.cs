using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Models.Authentications;
using UrlShortner.Application.Repositories;
using UrlShortner.Application.Settings;
using UrlShortner.Domain;

namespace UrlShortner.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly SaltKeySettings _saltKeySettings;
        private readonly ILogger<AuthService> _logger;

        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public AuthService(ILogger<AuthService> logger,
            IOptions<JwtSettings> jwtSettingsOption,
            IOptions<SaltKeySettings> saltKeySettingsOption,
            IUserCredentialRepository userCredentialRepository,
            IUserTokenRepository userTokenRepository)
        {
            _logger = logger;
            _saltKeySettings = saltKeySettingsOption.Value;
            _jwtSettings = jwtSettingsOption.Value;

            _userCredentialRepository = userCredentialRepository;
            _userTokenRepository = userTokenRepository;
        }

        public EncryptionResponse EncryptCredentials(string username, string password)
        {
            var encryptedEmail = GetHashedValue(username, _saltKeySettings.Username);
            var encryptedPassword = GetHashedValue(password, _saltKeySettings.Password);

            return new EncryptionResponse
            {
                Username = encryptedEmail,
                Password = encryptedPassword
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var hashedUsername = GetHashedValue(loginDto.UserName, _saltKeySettings.Username);
            var hashedPassword = GetHashedValue(loginDto.Password, _saltKeySettings.Password);
            var userCred = await _userCredentialRepository.FindAsync(x => x.Username == hashedUsername && x.Password == hashedPassword);

            if (userCred != null)
            {
                var user = userCred.User;
                var expiresIn = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiresMinutes);
                var token = GenerateToken(userCred.User!, expiresIn);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                await _userTokenRepository.AddAsync(new UserToken { Token = token, IPAddress = loginDto.IPAddress, RefreshToken = refreshToken, UserId = user.Id, RefreshTokenExpired = refreshTokenExpiration });

                return new LoginResponseDto()
                {
                    TokenType = "Bearer",
                    Token = token,
                    ExpiresIn = _jwtSettings.TokenExpiresMinutes * 60 * 60,
                    RefreshToken = refreshToken,
                };
            }
            return await Task.FromResult<LoginResponseDto>(null);
        }

        public async Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
        {
            var userToken = await _userTokenRepository.GetUserTokenAsync(x => x.Token == refreshTokenDto.Token && x.RefreshToken == refreshTokenDto.RefreshToken && x.IPAddress == refreshTokenDto.IPAddress);

            if (userToken is not null)
            {
                // var user = userToken.User;
                var expiresIn = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiresMinutes);
                var newToken = GenerateToken(userToken.User!, expiresIn);
                var newRefreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                userToken.Token = newToken;
                userToken.RefreshToken = newRefreshToken;
                userToken.RefreshTokenExpired = refreshTokenExpiration;

                await _userTokenRepository.UpdateAsync(userToken);

                return new LoginResponseDto()
                {
                    TokenType = "Bearer",
                    Token = newToken,
                    ExpiresIn = _jwtSettings.TokenExpiresMinutes * 60 * 60,
                    RefreshToken = newRefreshToken,
                };
            }

            return await Task.FromResult<LoginResponseDto>(null);
        }

        private string GenerateToken(User user, DateTime expiration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, "Developer" ),
                new Claim(ClaimTypes.Role, "Consultant" ),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: signInCredentials,
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        string GetHashedValue(string inputValue, string saltString)
        {
            byte[] salt = Encoding.ASCII.GetBytes(saltString);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(inputValue),
                salt,
                _jwtSettings.Iterations,
                hashAlgorithm,
                _jwtSettings.KeySize);

            return Convert.ToHexString(hash);
        }

        bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _jwtSettings.Iterations, hashAlgorithm, _jwtSettings.KeySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
