namespace DotNetRabbitMqTestContainersExample.RabbitMQ.Events;

public record CreatedPersonEvent(Guid Id, string Name, string Email);