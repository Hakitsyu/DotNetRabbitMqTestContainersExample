using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotNetRabbitMqTestContainersExample.API.Tests.Core;

public abstract class ProgramIntegrationTest(ProgramWebApplicationFactory webApplicationFactory)
    : IClassFixture<ProgramWebApplicationFactory>
{
    protected ProgramWebApplicationFactory WebApplicationFactory { get; } = webApplicationFactory;
    protected HttpClient Client { get; } = webApplicationFactory.CreateClient();

    protected IServiceScope ServiceScope { get; } = webApplicationFactory.Services.CreateScope();
    protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;
}