using DataAcessEFCore;
using DataAcessEFCore.Repositories;
using DataTypes.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RESTWebApi.Extensions
{
    public static class RepositoryRegister
    {
        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            // DB context for EF
            builder.Services.AddDbContext<ApplicationContext>(option =>
            {
                option.UseSqlite(builder.Configuration.GetSection("ConnectionStrings:TinyUrlDb").Value);
            });

            builder.Services.AddScoped<ApplicationContext>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUrlRepository, UrlRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        }
    }
}
