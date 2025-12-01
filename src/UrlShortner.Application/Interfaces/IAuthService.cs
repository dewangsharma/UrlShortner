using UrlShortner.Application.Models.Authentications;

namespace UrlShortner.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken);
    }
}
