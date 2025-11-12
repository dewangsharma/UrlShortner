using UrlShortner.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UrlShortner.Application.Repositories;

namespace UrlShortner.Infrastructure.Repositories
{
    public class UserCredentialRepository : GenericRepository<UserCredential>, IUserCredentialRepository
    {
        private readonly ApplicationContext _context;
        public UserCredentialRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserCredential?> FindAsync(Expression<Func<UserCredential, bool>> expression)
        {
            // return _context.UserCredentials.Where(expression).Include( x => x.User).AsNoTracking().FirstOrDefault();

            return await _context.UserCredentials
                .Include(uc => uc.User)
                    .ThenInclude(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(expression) ?? null;
        }
    }
}
