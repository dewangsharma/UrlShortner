using UrlShortner.Application.Repositories;
using UrlShortner.Domain;

namespace DataAcessEFCore.Repositories
{
    public class UrlRepository: GenericRepository<Url>, IUrlRepository
    {
        public UrlRepository(ApplicationContext context): base(context)
        {
                
        }
    }
}
