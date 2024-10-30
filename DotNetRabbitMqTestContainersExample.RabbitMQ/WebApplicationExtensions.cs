using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DotNetRabbitMqTestContainersExample.RabbitMQ;

public static class WebApplicationExtensions
{
    public static void ConfigureRabbitMq(this WebApplication webHost)
    {
        using var serviceScope = webHost.Services.CreateScope();
        using var connection = serviceScope.ServiceProvider.GetRequiredService<IConnection>();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(exchange: Exchanges.CreatedPerson, type: ExchangeType.Fanout);
    }
}