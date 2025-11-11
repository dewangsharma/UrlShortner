using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RESTWebApi.Extensions
{
    public static partial class AuthenticationRegister
    {
        public static void AuthWithJWTBearer(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                 .AddJwtBearer(jwt =>
                 {
                     jwt.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateLifetime = true,
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidAudience = builder.Configuration["JwtSettings:Audience"],
                         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                     };
                     jwt.Audience = builder.Configuration.GetSection("JwtSettings:Audience").Value;
                 });
        }
    }
}
