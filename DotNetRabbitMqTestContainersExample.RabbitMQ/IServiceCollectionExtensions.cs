using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace DotNetRabbitMqTestContainersExample.RabbitMQ;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        });

        services.AddScoped<IConnection>(serviceProvider => 
            serviceProvider.GetRequiredService<IConnectionFactory>().CreateConnection());
        
        return services;
    }
}