using Amazon.KeyManagementService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Repositories;
using UrlShortner.Infrastructure.Configurations;
using UrlShortner.Infrastructure.Repositories;
using UrlShortner.Infrastructure.Services;

namespace UrlShortner.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // TinyUrlDb connection-string and connection to database
            var connectionString = configuration.GetSection("ConnectionStrings:TinyUrlDb").Value;
            services.AddDbContext<ApplicationContext>(option =>
            {
                option.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 1,
                        maxRetryDelay: TimeSpan.FromSeconds(3),
                        errorNumbersToAdd: null));
            });

            // Register ApplicationContext & Repositories
            services.AddScoped<ApplicationContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUrlRepository, UrlRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            // Bind KMS settings
            var awsKmsConfiguration = configuration.GetSection("AWS:Kms");
            services.Configure<KmsSettings>(awsKmsConfiguration);

            var kmsSettings = awsKmsConfiguration.Get<KmsSettings>();

            // Register AWS KMS client (LocalStack or AWS depending on config)
            services.AddSingleton<IAmazonKeyManagementService>(sp =>
            {
                return new AmazonKeyManagementServiceClient(
                    kmsSettings.KeyId,
                    kmsSettings.SecretAccessKey,
                    new AmazonKeyManagementServiceConfig
                    {
                        ServiceURL = kmsSettings.ServiceURL,
                        UseHttp = true
                    }
                );
            });

            // Register encryption service
            services.AddTransient<IEncryptionService, AwsKmsEncryptionService>();
            services.AddTransient<IHashingService, HashingService>();

            return services;
        }
    }
}
