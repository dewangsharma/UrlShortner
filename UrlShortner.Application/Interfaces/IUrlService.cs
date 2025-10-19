using UrlShortner.Application.Models.Urls;

namespace UrlShortner.Application.Interfaces
{
    public interface IUrlService
    {
        Task<IEnumerable<UrlDto>> GetAllAsync(int userId = 0, CancellationToken token = default);

        Task<UrlDto?> GetSingleAsync(string actualUrl, CancellationToken token);

        Task<UrlDto?> GetSingleByAliasAsync(string alias, CancellationToken token);

        Task<UrlDto> CreateAsync(UrlCreateDto urlCreate, CancellationToken token);

        Task<UrlDto> UpdateAsync(UrlUpdateDto urlUpdate, CancellationToken token);

    }
}
