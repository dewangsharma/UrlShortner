using UrlShortner.Application.Repositories;
using UrlShortner.Domain;

namespace DataAcessEFCore.Repositories
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context): base(context)
        {       
        }
    }
}
