using UrlShortner.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using UrlShortner.Application.Repositories;

namespace DataAcessEFCore.Repositories
{
    public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
    {
        private readonly ApplicationContext _context;

        public UserTokenRepository(ApplicationContext context): base(context)
        {
            _context = context;
        }

        public async Task<UserToken> GetUserTokenAsync(Expression<Func<UserToken, bool>> expression)
        {
            return _context.UserTokens.Where(expression).Include(x => x.User).AsNoTracking().FirstOrDefault();
        }
    }
}
