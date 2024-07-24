using DataTypes;
using DataTypes.Repositories;

namespace DataAcessEFCore.Repositories
{
    public class UrlRepository: GenericRepository<Url>, IUrlRepository
    {
        public UrlRepository(ApplicationContext context): base(context)
        {
                
        }
    }
}
