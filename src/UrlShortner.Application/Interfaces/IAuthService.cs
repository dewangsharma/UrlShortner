using UrlShortner.Application.Models.Authentications;

namespace UrlShortner.Application.Interfaces
{
    public interface IAuthService
    {
        (string Email,string Password) EncryptCredentials(string username, string password);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
