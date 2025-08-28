using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an operation in Nexus Domain Studio.
/// </summary>
public class NDSOperation : Entity<string>, INDSContextObject
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    
    public List<NDSArgument> Arguments { get; private set; }
    public List<NDSOperationEmit> Emits { get; private set; }

    /// <summary>
    /// Creates a new NDSOperation instance with the specified name and optional description.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="arguments"></param>
    /// <param name="emits"></param>
    private NDSOperation(string name, string? description, List<NDSArgument> arguments, List<NDSOperationEmit> emits) 
    {
        Name = name;
        Description = description;
        Arguments = arguments;
        Emits = emits;
    }

    /// <summary>
    /// Creates a new NDSOperation instance with the specified name, optional description, arguments, and emits.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="arguments"></param>
    /// <param name="emits"></param>
    /// <returns></returns>
    public static Result<NDSOperation> Create(string name, string? description, List<NDSArgument> arguments, List<NDSOperationEmit> emits)
    {
        // Validate the name of the operation
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSOperation>.Error("Operation name cannot be empty.");
        }

        // Create the NDSOperation instance
        var operation = new NDSOperation(name, description, arguments, emits);

        // Return the created operation
        return operation;
    }

    /// <summary>
    /// Gets the type of the context object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Operation;
}