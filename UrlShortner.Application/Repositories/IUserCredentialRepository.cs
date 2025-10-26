using System.Linq.Expressions;
using UrlShortner.Domain;

namespace UrlShortner.Application.Repositories
{
    public interface IUserCredentialRepository: IGenericRepository<UserCredential>
    {
        Task<UserCredential> FindAsync(Expression<Func<UserCredential, bool>> expression);
    }
}
