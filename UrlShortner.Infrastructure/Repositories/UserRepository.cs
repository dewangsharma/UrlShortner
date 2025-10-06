using DataTypes;
using DataTypes.Repositories;

namespace DataAcessEFCore.Repositories
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context): base(context)
        {       
        }
    }
}
