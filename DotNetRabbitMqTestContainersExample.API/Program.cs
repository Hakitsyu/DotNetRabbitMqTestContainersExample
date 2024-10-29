using System.Text;
using System.Text.Json;
using DotNetRabbitMqTestContainersExample.API;
using DotNetRabbitMqTestContainersExample.API.Contracts;
using DotNetRabbitMqTestContainersExample.RabbitMQ;
using DotNetRabbitMqTestContainersExample.RabbitMQ.Events;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbitMq(builder.Configuration.GetConnectionString("RabbitMQ")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureRabbitMq();

app.MapPost("/person", (CreatePersonDto content, [FromServices] IConnection connection) =>
    {
        var id = Guid.NewGuid();

        using var channel = connection.CreateModel();

        var evt = new CreatedPersonEvent(id, content.Name, content.Email);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
        
        channel.BasicPublish(exchange: Exchanges.CREATED_PERSON,
            routingKey: string.Empty,
            body: body);
    })
    .WithName("Person")
    .WithOpenApi();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
}
#pragma warning restore CA1050 // Declare types in namespaces