using UrlShortner.Application.Models.Authentications;

namespace UrlShortner.Application.Interfaces
{
    public interface IAuthService
    {
        EncryptionResponse EncryptCredentials(string username, string password);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken);
    }
}
