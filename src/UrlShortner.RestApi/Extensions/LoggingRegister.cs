using Serilog;

namespace RESTWebApi.Extensions
{
    public static class LoggingRegister
    {
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) => {
                configuration.ReadFrom.Configuration(context.Configuration);           
            });
        }
    }
}
