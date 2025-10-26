using Microsoft.Extensions.Options;
using UrlShortner.Domain.Options;

namespace RESTWebApi.Options
{
    public class JwtOptionSetup : IConfigureOptions<JwtOption>
    {
        private const string ConfigSection = "JWT";
        private readonly IConfiguration _configuration;

        public JwtOptionSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOption options)
        {
            _configuration.GetSection(ConfigSection).Bind(options);
        }
    }
}
