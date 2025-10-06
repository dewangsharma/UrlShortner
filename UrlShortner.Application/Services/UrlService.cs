using BusinessLayer.Interfaces;
using DataTypes;
using DataTypes.Mappers;
using DataTypes.Repositories;
using DataTypes.Requests;
using DataTypes.Responses;

namespace BusinessLayer.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<IEnumerable<UrlRes>> GetAllAsync(int userId, CancellationToken token)
        {
            if (userId > 0)
            {
                var result = await _urlRepository.FindAsync(x => x.UserId == userId);
                return result == null ? Enumerable.Empty<UrlRes>() :  result.ToDto();
            }
            
            return Enumerable.Empty<UrlRes>();
        }

        public async Task<UrlRes?> GetSingleAsync(string actualUrl, CancellationToken token)
        {
            return (await _urlRepository.GetSingleAsync(x => x.Actual.ToLower() == actualUrl.ToLower()))?.ToDto() ?? default;
        }

        public async Task<UrlRes?> GetSingleByAliasAsync(string alias, CancellationToken token)
        {
            return (await _urlRepository.GetSingleAsync(x => x.Shortened.ToLower() == alias.ToLower()))?.ToDto() ?? default;
        }

        public async Task<UrlRes> CreateAsync(string actualUrl, int userId, CancellationToken token)
        {
            var url = await GetSingleAsync(actualUrl, token);

            // validate the existing actualurl 
            if (url is null)
            {
                var shortnedUrl = await GetUrlShortner();
                var result = await _urlRepository.AddAsync(new Url { UserId = userId, Status = UrlStatus.Active, Actual = actualUrl, Shortened = shortnedUrl });
                return result.ToDto();
            }

            throw new Exception($"Url {url.Actual} already exist with Shortned url {url.Shortened}");
        }

        public async Task<UrlRes> UpdateAsync(UrlUpdateReq request, CancellationToken token)
        {
            var url = await _urlRepository.GetByIdAsync(request.Id);
            if (url is null)
            {
                throw new Exception($"Existing Url not found for the Id: {request.Id}");
            }

            url.Status = request.Status;

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
