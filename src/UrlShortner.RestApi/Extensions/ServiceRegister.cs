using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Services;

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
        }
    }
}
