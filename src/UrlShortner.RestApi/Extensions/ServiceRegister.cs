using Microsoft.Extensions.Caching.Memory;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Services;
using UrlShortner.RestApi.Securities;

namespace RESTWebApi.Extensions
{
    public static class ServiceRegister
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            //builder.Services.AddSingleton<IAuthService, AuthService>(p =>
            //{
            //    return new AuthService(config: builder.Configuration);
            //});

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUrlService, UrlService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // builder.Services.Decorate<IAuthService, LockoutAuthServiceDecorator>();

            /*
            builder.Services.AddScoped<AuthService>();

            // register decorator as the IAuthService implementation
            builder.Services.AddScoped<IAuthService>(sp =>
                new UrlShortner.RestApi.Securities.LockoutAuthServiceDecorator(
                    sp.GetRequiredService<AuthService>(),
                    sp.GetRequiredService<IMemoryCache>()
                ));
            */
        }
    }
}
