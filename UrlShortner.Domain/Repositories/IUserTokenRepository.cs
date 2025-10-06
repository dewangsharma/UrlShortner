using System.Linq.Expressions;

namespace DataTypes.Repositories
{
    public interface IUserTokenRepository : IGenericRepository<UserToken>
    {
        Task<UserToken> GetUserTokenAsync(Expression<Func<UserToken, bool>> expression);
    }
}
