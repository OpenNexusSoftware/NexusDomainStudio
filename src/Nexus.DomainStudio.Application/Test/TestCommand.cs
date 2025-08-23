using Nexus.DomainStudio.Application.CQRS.Abstraction;
using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Application.Test;

// Command
public record TestCommand(string Name, int Age) : ICommand<string>;

// Command Handler
public class TestCommandHandler : ICommandHandler<TestCommand, string>
{
    Task<Result<string>> ICommandHandler<TestCommand, string>.HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        // Simulate some processing logic
        if (command.Age < 0)
        {
            return Task.FromResult(Result<string>.Error("Age cannot be negative"));
        }

        // Return a successful result
        return Task.FromResult(Result<string>.Error($"Hello {command.Name}, you are {command.Age} years old!"));
    }
}