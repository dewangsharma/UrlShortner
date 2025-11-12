using FluentValidation;
using FluentValidation.AspNetCore;
using UrlShortner.RestApi.Models.Authentications;

namespace UrlShortner.RestApi.Extensions
{
    public static class FluentValidationRegister
    {
        public static void AddFluentValidations(this WebApplicationBuilder builder) 
        {
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RefreshTokenRequestValidator>();
        }
    }
}
