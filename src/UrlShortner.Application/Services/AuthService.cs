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
        private readonly IEncryptionService _encryptionService;
        private readonly IHashingService _hashingService;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger,
            IOptions<JwtSettings> jwtSettingsOption,
            IEncryptionService encryptionService,
            IHashingService hashingService,
            IUserCredentialRepository userCredentialRepository,
            IUserTokenRepository userTokenRepository)
        {
            _logger = logger;
            _jwtSettings = jwtSettingsOption.Value;

            _hashingService = hashingService;
            _encryptionService = encryptionService;
            _userCredentialRepository = userCredentialRepository;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                if (loginDto is null)
                {
                    _logger.LogWarning("Login attempt with null payload");
                    return null;
                }

                var encryptedUsername = await _encryptionService.EncryptAsync(loginDto.UserName);
                var userCred = await _userCredentialRepository.FindAsync(x => x.Username == encryptedUsername);

                if (userCred == null)
                {
                    _logger.LogWarning("User not found for username {Username} from IP {IP}", loginDto.UserName, loginDto.IPAddress);
                    return null; // Returning null when no user found
                }

                bool isValid = _hashingService.Verify(loginDto.Password, userCred.Password);

                if (!isValid)
                {
                    _logger.LogWarning("Invalid password for user from IP {IP}", loginDto.IPAddress);
                }

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
                    ExpiresIn = _jwtSettings.TokenExpiresMinutes * 60,
                    RefreshToken = refreshToken,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Username} from IP {IP}", loginDto.UserName, loginDto.IPAddress);
                throw;
            }
        }

        public async Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
        {
            if (refreshTokenDto is null)
            {
                _logger.LogWarning("Refresh token request with null payload");
                return null;
            }

            var userToken = await _userTokenRepository.GetUserTokenAsync(x => x.Token == refreshTokenDto.Token && x.RefreshToken == refreshTokenDto.RefreshToken && x.IPAddress == refreshTokenDto.IPAddress);

            if (userToken is null)
            {
                _logger.LogWarning("Refresh token not found for token from IP {IP}", refreshTokenDto.IPAddress);
                return null;
            }

            if (userToken.RefreshTokenExpired <= DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token expired for UserId {UserId} from IP {IP}", userToken.UserId, refreshTokenDto.IPAddress);
                return null;
            }

            
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
                    ExpiresIn = _jwtSettings.TokenExpiresMinutes * 60,
                    RefreshToken = newRefreshToken,
                };
        }

        private string GenerateToken(User user, DateTime expiration)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(_jwtSettings.SecretKey)) throw new InvalidOperationException("JWT secret key is not configured.");
            
            // Use UTF8 (safer for non-ASCII secrets) and ensure key length is sufficient for HMACSHA256 (>= 256 bits)
            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            if (keyBytes.Length < 32)
            {
                _logger.LogWarning("Configured JWT secret is shorter than 256 bits. This is insecure.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(user.UserRoles.Where(x => x.Role != null && !string.IsNullOrWhiteSpace(x.Role.Name))
                .Select(x => new Claim(ClaimTypes.Role, x.Role.Name)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
