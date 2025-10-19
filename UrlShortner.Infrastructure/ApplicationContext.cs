using UrlShortner.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAcessEFCore
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}