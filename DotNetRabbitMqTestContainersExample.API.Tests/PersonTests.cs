using System.Net;
using System.Text;
using System.Text.Json;
using DotNetRabbitMqTestContainersExample.API.Contracts;
using DotNetRabbitMqTestContainersExample.API.Tests.Core;
using DotNetRabbitMqTestContainersExample.RabbitMQ;
using DotNetRabbitMqTestContainersExample.RabbitMQ.Events;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotNetRabbitMqTestContainersExample.API.Tests;

public sealed class PersonTests(ProgramWebApplicationFactory webApplicationFactory) 
    : ProgramIntegrationTest(webApplicationFactory)
{
    [Theory]
    [InlineData("John Doe", "john@doe.com")]
    public async Task Person_Create_ShouldPublishCreatedPersonEvent(string name, string email)
    {
        using var connection = ServiceProvider.GetRequiredService<IConnection>();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(string.Empty, false, false, false, null);
        channel.QueueBind(string.Empty, Exchanges.CREATED_PERSON, "", null);

        CreatedPersonEvent? evt = null;
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, args) =>
        {
            evt = JsonSerializer.Deserialize<CreatedPersonEvent>(
                Encoding.UTF8.GetString(args.Body.ToArray()));
        };
        
        channel.BasicConsume(queue: string.Empty, autoAck: true, consumer: consumer);
        
        var body = new CreatePersonDto(Name: name, LastName: null, Email: email);

        var content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json"
        );
        
        var response = await Client.PostAsync("person", content);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(evt);
        Assert.Equal(email, evt.Email);
        Assert.Equal(name, evt.Name);
    }
}