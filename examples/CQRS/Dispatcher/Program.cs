using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nexus.DomainStudio.Application.CQRS;
using Nexus.DomainStudio.Application.CQRS.Abstraction;
using Nexus.DomainStudio.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

#region Entrypoint

var services = new ServiceCollection();

// Register core CQRS infrastructure
services.AddScoped<IDispatcher, Dispatcher>();
services.AddScoped<ICommandHandler<TestCommand, string>, TestCommandHandler>();
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

var provider = services.BuildServiceProvider();
var dispatcher = provider.GetRequiredService<IDispatcher>();

// Execute a test command
var command = new TestCommand("John Doe", 42);
var result = await dispatcher.DispatchCommand(command);

Console.WriteLine($"Command Result: {result.IsSuccess}");
Console.WriteLine($"Value: {result.Value}");

#endregion

#region "CQRS Dispatcher Objects"

// Command
public record TestCommand(string Name, int Age) : ICommand<string>;

// Command Handler
public class TestCommandHandler : ICommandHandler<TestCommand, string>
{
    public Task<Result<string>> HandleAsync(TestCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || command.Age <= 0)
            return Task.FromResult(Result<string>.Error("Invalid input"));

        return Task.FromResult(Result<string>.Success($"Hello {command.Name}, you're {command.Age} years old!"));
    }
}

// Pipeline Behavior
public class LoggingBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes>
    where TReq : notnull
    where TRes : notnull
{
    public async Task<Result<TRes>> HandleAsync(TReq request, PipelineDelegate<TRes> next, CancellationToken ct)
    {
        Console.WriteLine($"[Pipeline] → Handling: {typeof(TReq).Name}");
        var result = await next(ct);
        Console.WriteLine($"[Pipeline] ← Finished: {typeof(TReq).Name}");
        return result;
    }
}

#endregion


#region "Example Command and Handler"

// public record TestCommand : ICommand<string>
// {
//     public string Name { get; init; } = string.Empty;
//     public int Age { get; init; }
// }

// public class TestCommandHandler : ICommandHandler<TestCommand, string>
// {
//     public Task<Result<string>> HandleAsync(TestCommand command, CancellationToken cancellationToken)
//     {
//         Console.WriteLine("Command is being executed....");

//         // Simulate some processing logic
//         if (string.IsNullOrWhiteSpace(command.Name) || command.Age <= 0)
//         {
//             return Task.FromResult(Result<string>.Error("Invalid command parameters."));
//         }

//         // Return a successful result with a message
//         var resultMessage = $"Command processed successfully: Name = {command.Name}, Age = {command.Age}";
//         return Task.FromResult(Result<string>.Success(resultMessage));
//     }
// }

// public class GenericPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : notnull
//     where TResponse : notnull
// {
//     public async Task<Result<TResponse>> HandleAsync(TRequest request, PipelineDelegate<TResponse> next, CancellationToken cancellationToken)
//     {
//         Console.WriteLine($"Processing request of type {typeof(TRequest).Name}...");
//         // Call the next handler in the pipeline
//         var result = await next(cancellationToken);
//         // You can add additional logic here, such as logging or validation
//         Console.WriteLine($"Finished processing request of type {typeof(TRequest).Name}.");

//         return result;
//     }
// }

#endregion
