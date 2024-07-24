using RESTWebApi.Exceptions;
using RESTWebApi.Extensions;

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
            builder.AuthWithJWTBearer();
            builder.AddServices();
            builder.AddRepositories();
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin",
                    options => // options.AllowAnyOrigin()
                    options.WithOrigins("http://localhost:4200/", "https://localhost:44351", "**")
                    .AllowAnyHeader()
                    .AllowAnyMethod().AllowAnyOrigin()
                    );
            });
            var app = builder.Build();

            app.UseMiddleware<GlobalCancellationException>();
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