using BusinessLayer.Interfaces;
using DataTypes;
using DataTypes.Repositories;
using DataTypes.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;
        private readonly string _usernameSalt;
        private readonly string _passwordSalt;
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        const int keySize = 64;
        const int iterations = 350000;
        const int tokenExpiresMinutes = 15;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        // IConfiguration
        public AuthService(IConfiguration config, 
            IUserCredentialRepository userCredentialRepository,
            IUserTokenRepository userTokenRepository)
        {
            _audience = config.GetSection("JWT:Audience").Value ?? throw new KeyNotFoundException("JWT:Audience");
            _issuer = config.GetSection("JWT:Issuer").Value ?? throw new KeyNotFoundException("JWT:Issuer");
            _key = config.GetSection("JWT:Key").Value ?? throw new KeyNotFoundException("JWT:Key");
            _usernameSalt = config.GetSection("SaltKey:Username").Value ?? throw new KeyNotFoundException("SaltKey:Username");
            _passwordSalt = config.GetSection("SaltKey:Password").Value ?? throw new KeyNotFoundException("SaltKey:Password");
            _userCredentialRepository = userCredentialRepository;
            _userTokenRepository = userTokenRepository;
        }

        public (string Username, string Password) EncryptCredentials(string username, string password)
        {
            return (GetHashedValue(username, _usernameSalt), GetHashedValue(password, _passwordSalt));
        }

        public async Task<AuthenticationRes> LoginAsync(string username, string password, string ipAddress)
        {
            var hashedUsername = GetHashedValue(username, _usernameSalt);
            var hashedPassword = GetHashedValue(password, _passwordSalt);
            var userCred = await _userCredentialRepository.FindAsync(x => x.Username == hashedUsername && x.Password == hashedPassword);

            if (userCred != null)
            {
                var user = userCred.User;
                var expiresIn = DateTime.UtcNow.AddMinutes(tokenExpiresMinutes);
                var token = GenerateToken(userCred.User!, expiresIn);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

                await _userTokenRepository.AddAsync(new UserToken { Token = token, IPAddress = ipAddress, RefreshToken = refreshToken, UserId = user.Id, RefreshTokenExpired = refreshTokenExpiration });
                
                return new AuthenticationRes()
                {
                    TokenType = "Bearer",
                    Token = token,
                    ExpiresIn = tokenExpiresMinutes * 60 * 60,
                    RefreshToken = refreshToken,
                };
            }
            return await Task.FromResult<AuthenticationRes>(null);
        }

        public async Task<AuthenticationRes> GetRefreshToken(string token, string refreshToken, string ipAddress)
        {
            var userToken = await _userTokenRepository.GetUserTokenAsync(x => x.Token == token && x.RefreshToken == refreshToken && x.IPAddress == ipAddress);

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

                return new AuthenticationRes()
                {
                    TokenType = "Bearer",
                    Token = newToken,
                    ExpiresIn = tokenExpiresMinutes * 60 * 60,
                    RefreshToken = newRefreshToken,
                };
            }

            return await Task.FromResult<AuthenticationRes>(null);
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

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: signInCredentials,
                issuer: _issuer,
                audience: _audience
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
