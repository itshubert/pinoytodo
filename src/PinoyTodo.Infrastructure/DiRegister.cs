using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PinoyCleanArch;
using PinoyTodo.Application.Common.Interfaces;
using PinoyTodo.Infrastructure.Messaging;
using PinoyTodo.Infrastructure.Persistence;
using PinoyTodo.Infrastructure.Persistence.Repositories;

namespace PinoyTodo.Infrastructure;

public static partial class DiRegister
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EventStoreDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("EventStoreDbContext");
            options.UseNpgsql(connectionString);
        });

        services.AddMessaging(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ISqsClient, SqsClient>();

        services.AddInfrastructure();
        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AWSSettings>(configuration.GetSection("AWSSettings"));
        services.Configure<QueueSettings>(configuration.GetSection("QueueSettings"));
        var logger = services.BuildServiceProvider().GetRequiredService<Microsoft.Extensions.Logging.ILogger<SqsClient>>();

        services.AddSingleton<IAmazonSQS>(sp =>
        {
            var awsSettings = sp.GetRequiredService<IOptions<AWSSettings>>().Value;

            if (awsSettings.UseLocalStack)
            {
                var serviceUrl = !string.IsNullOrEmpty(awsSettings.ServiceUrl)
                    ? awsSettings.ServiceUrl
                    : "http://localstack:4566";

                // serviceUrl = "http://localstack:4566";

                logger.LogInformation($"LOCALSTACK SERVICEURL at {serviceUrl}");

                var sqsConfig = new AmazonSQSConfig { ServiceURL = serviceUrl };
                return new AmazonSQSClient(new AnonymousAWSCredentials(), sqsConfig);
            }

            var profileName = Environment.GetEnvironmentVariable("AWS_PROFILE");

            if (!string.IsNullOrEmpty(profileName))
            {
                var credentialProfileStoreChain = new CredentialProfileStoreChain();
                if (credentialProfileStoreChain.TryGetProfile(profileName, out var profile))
                {
                    var credentials = profile.GetAWSCredentials(credentialProfileStoreChain);
                    return new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(awsSettings.Region));
                }
            }


            return new AmazonSQSClient(RegionEndpoint.GetBySystemName(awsSettings.Region));
        });

        return services;
    }
}