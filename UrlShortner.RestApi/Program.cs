using DataAcessEFCore;
using Microsoft.EntityFrameworkCore;
using RESTWebApi.Exceptions;
using RESTWebApi.Extensions;
using RESTWebApi.Options;
using UrlShortner.Infrastructure;

namespace RESTWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.AddServices();
            builder.AddLogging();
            builder.Services.ConfigureOptions<JwtOptionSetup>();
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

            app.UseMiddleware<GlobalCancellationException>();

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