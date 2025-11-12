using Microsoft.EntityFrameworkCore;
using RESTWebApi.Exceptions;
using RESTWebApi.Extensions;
using UrlShortner.Application.Settings;
using UrlShortner.Infrastructure;
using UrlShortner.RestApi.Extensions;
using UrlShortner.RestApi.Securities;

namespace RESTWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Accept JSON case insensitive
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            // FluentValidation
            builder.AddFluentValidations();

            // Configure Redis Cache
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.Configure<SaltKeySettings>(builder.Configuration.GetSection("SaltKeySettings"));
            builder.Services.AddSwaggerGen();

            builder.AddServices();
            builder.AddLogging();
            builder.AuthWithJWTBearer();

            // builder.AddRepositories();

            builder.Services.AddInfrastructure(builder.Configuration.GetSection("ConnectionStrings:TinyUrlDb").Value);

            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin",
                    options => // options.AllowAnyOrigin()
                    options.WithOrigins("http://localhost:4200/", "https://localhost:44351", "**")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    );
            });
            var app = builder.Build();

            app.UseMiddleware<RateLimitMiddleware>();

            app.UseMiddleware<GlobalCancellationException>();

            // later in pipeline, after app is built:
            // app.UseRateLimiter();

            // builder.Services.AddMemoryCache();

            // Apply migrations automatically on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var pendingMigrations = db.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending EF Core migrations...");
                    db.Database.Migrate();
                    Console.WriteLine("Database is up to date.");
                }
                else
                {
                    Console.WriteLine("No pending migrations found.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowOrigin");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}