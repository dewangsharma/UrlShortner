using UrlShortner.Application.Repositories;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Mappers;
using UrlShortner.Application.Models.Urls;

namespace UrlShortner.Application.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<IEnumerable<UrlDto>> GetAllAsync(int userId, CancellationToken token)
        {
            if (userId > 0)
            {
                var result = await _urlRepository.FindAsync(x => x.UserId == userId);
                return result == null ? Enumerable.Empty<UrlDto>() :  result.ToDto();
            }
            
            return Enumerable.Empty<UrlDto>();
        }

        public async Task<UrlDto?> GetSingleAsync(string actualUrl, CancellationToken token)
        {
            return (await _urlRepository.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower()))?.ToDto() ?? default;
        }

        public async Task<UrlDto?> GetSingleByAliasAsync(string alias, CancellationToken token)
        {
            return (await _urlRepository.GetSingleAsync(x => x.Shortened.ToLower() == alias.ToLower()))?.ToDto() ?? default;
        }

        public async Task<UrlDto> CreateAsync(UrlCreateDto urlCreate, CancellationToken token)
        {
            var url = await GetSingleAsync(urlCreate.Actual, token);

            // validate the existing actualurl 
            if (url is null)
            {
                var shortnedUrl = await GetUrlShortner();
                var result = await _urlRepository.AddAsync(urlCreate.ToDomain(shortnedUrl));
                // var result = await _urlRepository.AddAsync(new Url { UserId = urlCreate.UserId, Status = UrlStatus.Active, Actual = urlCreate.Actual, Shortened = shortnedUrl });
                return result.ToDto();
            }

            throw new Exception($"Url {url.Actual} already exist with Shortned url {url.Shortened}");
        }

        public async Task<UrlDto> UpdateAsync(UrlUpdateDto urlUpdateDto, CancellationToken token)
        {
            var url = await _urlRepository.GetByIdAsync(urlUpdateDto.Id);
            if (url is null)
            {
                throw new Exception($"Existing Url not found for the Id: {urlUpdateDto.Id}");
            }

            url.Status = urlUpdateDto.Status.ToDomain();

            var updatedUrl =  await _urlRepository.UpdateAsync(url);
            if (updatedUrl is null)
            {
                throw new Exception($"Something went wrong in updatin the Url, Id: {url.Id}");
            }

            return updatedUrl.ToDto();
        }

        private async Task<string> GetUrlShortner()
        {
            var random =  ShortnerGenerator.GenerateRandom(6);
            var isUnique = await _urlRepository.GetSingleAsync(x => x.Shortened.ToLower() == random.ToLower());
            if (isUnique is null)
                return random;
            else
                return await GetUrlShortner();
        }
    }
}
