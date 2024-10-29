using System.ComponentModel.DataAnnotations;

namespace DotNetRabbitMqTestContainersExample.API.Contracts;

public record CreatePersonDto(string Name, string? LastName, [Required] [EmailAddress] string Email);