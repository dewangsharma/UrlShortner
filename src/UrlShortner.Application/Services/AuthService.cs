using Microsoft.Extensions.Configuration;
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
using UrlShortner.Domain;
using UrlShortner.Domain.Options;

namespace UrlShortner.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _usernameSalt;
        private readonly string _passwordSalt;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        const int keySize = 64;
        const int iterations = 350000;
        const int tokenExpiresMinutes = 15;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly ILogger<AuthService> _logger;

        private readonly JwtOption _jwtOption;

        public AuthService(IConfiguration config,
            IUserCredentialRepository userCredentialRepository,
            IUserTokenRepository userTokenRepository,
            IOptions<JwtOption> option,
            ILogger<AuthService> logger)
        {
            _usernameSalt = config.GetSection("SaltKey:Username").Value ?? throw new KeyNotFoundException("SaltKey:Username");
            _passwordSalt = config.GetSection("SaltKey:Password").Value ?? throw new KeyNotFoundException("SaltKey:Password");
            _userCredentialRepository = userCredentialRepository;
            _userTokenRepository = userTokenRepository;
            _logger = logger;

            _jwtOption = option.Value;
        }

        public (string Email, string Password) EncryptCredentials(string username, string password)
        {
            return (GetHashedValue(username, _usernameSalt), GetHashedValue(password, _passwordSalt));
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var hashedUsername = GetHashedValue(loginDto.UserName, _usernameSalt);
            var hashedPassword = GetHashedValue(loginDto.Password, _passwordSalt);
            var userCred = await _userCredentialRepository.FindAsync(x => x.Username == hashedUsername && x.Password == hashedPassword);

            if (userCred != null)
            {
                var user = userCred.User;
                var expiresIn = DateTime.UtcNow.AddMinutes(tokenExpiresMinutes);
                var token = GenerateToken(userCred.User!, expiresIn);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                await _userTokenRepository.AddAsync(new UserToken { Token = token, IPAddress = loginDto.IPAddress, RefreshToken = refreshToken, UserId = user.Id, RefreshTokenExpired = refreshTokenExpiration });

                return new LoginResponseDto()
                {
                    TokenType = "Bearer",
                    Token = token,
                    ExpiresIn = tokenExpiresMinutes * 60 * 60,
                    RefreshToken = refreshToken,
                };
            }
            return await Task.FromResult<LoginResponseDto>(null);
        }

        public async Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var userToken = await _userTokenRepository.GetUserTokenAsync(x => x.Token == refreshTokenDto.Token && x.RefreshToken == refreshTokenDto.RefreshToken && x.IPAddress == refreshTokenDto.IPAddress);

            if (userToken is not null)
            {
                // var user = userToken.User;
                var expiresIn = DateTime.UtcNow.AddMinutes(tokenExpiresMinutes);
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
                    ExpiresIn = tokenExpiresMinutes * 60 * 60,
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

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOption.SecretKey));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: signInCredentials,
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        string GetHashedValue(string inputValue, string saltString)
        {
            byte[] salt = Encoding.ASCII.GetBytes(saltString);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(inputValue),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
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
