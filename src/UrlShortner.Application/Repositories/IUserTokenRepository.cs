using System.Linq.Expressions;
using UrlShortner.Domain;

namespace UrlShortner.Application.Repositories
{
    public interface IUserTokenRepository : IGenericRepository<UserToken>
    {
        Task<UserToken> GetUserTokenAsync(Expression<Func<UserToken, bool>> expression);
    }
}
