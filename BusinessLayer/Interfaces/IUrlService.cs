using DataTypes;
using DataTypes.Requests;
using DataTypes.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IUrlService
    {
        Task<IEnumerable<UrlRes>> GetAllAsync(int userId = 0, CancellationToken token = default);

        Task<UrlRes?> GetSingleAsync(string actualUrl, CancellationToken token);

        Task<UrlRes?> GetSingleByAliasAsync(string alias, CancellationToken token);

        Task<UrlRes> CreateAsync(string actualUrl, int userId, CancellationToken token);

        Task<UrlRes> UpdateAsync(UrlUpdateReq request, CancellationToken token);

    }
}
