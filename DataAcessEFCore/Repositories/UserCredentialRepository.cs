using DataTypes;
using DataTypes.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAcessEFCore.Repositories
{
    public class UserCredentialRepository : GenericRepository<UserCredential>, IUserCredentialRepository
    {
        private readonly ApplicationContext _context;
        public UserCredentialRepository(ApplicationContext context): base(context)
        {
            _context = context;
        }

        public async Task<UserCredential> FindAsync(Expression<Func<UserCredential, bool>> expression)
        {
            return _context.UserCredentials.Where(expression).Include( x => x.User).AsNoTracking().FirstOrDefault();
        }
    }
}
