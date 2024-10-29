using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.RabbitMq;

namespace DotNetRabbitMqTestContainersExample.API.Tests.Core;

public sealed class ProgramWebApplicationFactory : WebApplicationFactory<Program>
{
    public RabbitMqContainer RabbitMqContainer { get; } = new RabbitMqBuilder().Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        RabbitMqContainer.StartAsync().Wait();
        
        Environment.SetEnvironmentVariable("ConnectionStrings__RabbitMq", RabbitMqContainer.GetConnectionString());
    }
}