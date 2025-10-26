using DataAcessEFCore;
using DataAcessEFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortner.Application.Repositories;

namespace UrlShortner.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            //// Register repository implementations
            //services.AddScoped<IUrlRepository, UrlRepository>();


            // DB context for EF
            services.AddDbContext<ApplicationContext>(option =>
            {
                // option.UseSqlite(connectionString);
                option.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 1,
                        maxRetryDelay: TimeSpan.FromSeconds(3),
                        errorNumbersToAdd: null));
            });

            services.AddScoped<ApplicationContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUrlRepository, UrlRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            return services;
        }
    }
}
