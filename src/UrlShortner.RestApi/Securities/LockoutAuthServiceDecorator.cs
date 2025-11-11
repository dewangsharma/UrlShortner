using Microsoft.Extensions.Caching.Memory;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Models.Authentications;

namespace UrlShortner.RestApi.Securities
{
    public class LockoutAuthServiceDecorator : IAuthService
    {
        private readonly IAuthService _inner;
        private readonly IMemoryCache _cache;

        private const int MaxFailedAttempts = 5;
        private static readonly TimeSpan FailureWindow = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

        public LockoutAuthServiceDecorator(IAuthService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public EncryptionResponse EncryptCredentials(string username, string password)
            => _inner.EncryptCredentials(username, password);

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var userKey = $"lockout:user:{loginDto.UserName}";
            if (_cache.TryGetValue(userKey, out bool isLocked) && isLocked)
            {
                // Locked out
                return null;
            }

            var result = await _inner.LoginAsync(loginDto, cancellationToken);
            if (result == null)
            {
                // record failure
                var failuresKey = $"failures:user:{loginDto.UserName}";
                var failures = _cache.Get<int?>(failuresKey) ?? 0;
                failures++;
                _cache.Set(failuresKey, failures, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = FailureWindow
                });

                if (failures >= MaxFailedAttempts)
                {
                    _cache.Set(userKey, true, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = LockoutDuration
                    });
                }

                return null;
            }

            // success: clear failure counters
            _cache.Remove($"failures:user:{loginDto.UserName}");
            _cache.Remove($"lockout:user:{loginDto.UserName}");
            return result;
        }

        public async Task<LoginResponseDto> GetRefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
        {
            // Optional: track refresh token abuse by refresh token or IP
            var result = await _inner.GetRefreshToken(refreshTokenDto, cancellationToken);
            // If you want to lock based on refresh token misuse, implement like LoginAsync above.
            return result;
        }
    }
}
