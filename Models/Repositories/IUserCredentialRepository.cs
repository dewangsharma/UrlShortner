using System.Linq.Expressions;

namespace DataTypes.Repositories
{
    public interface IUserCredentialRepository: IGenericRepository<UserCredential>
    {
        Task<UserCredential> FindAsync(Expression<Func<UserCredential, bool>> expression);
    }
}
