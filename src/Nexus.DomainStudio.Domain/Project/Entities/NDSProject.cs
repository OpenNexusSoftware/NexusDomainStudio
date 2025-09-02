using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents a project in Nexus Domain Studio.
/// </summary>
public sealed class NDSProject : AggregateRoot<string>
{
    /// <summary>
    /// Read-only access to the contexts in the project.
    /// </summary>
    public NDSProjectDetails Details { get; private set; }

    // Backing field for contexts
    private readonly Dictionary<string, NDSContext> _contexts;

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="details"></param>
    /// <param name="contexts"></param>
    private NDSProject(string id, NDSProjectDetails details) : base(id)
    {
        Details = details;
        _contexts = [];
    }

    /// <summary>
    /// Read-only access to the contexts in the project.
    /// </summary>
    public IReadOnlyDictionary<string, NDSContext> Contexts => _contexts.AsReadOnly();

    /// <summary>
    /// Creates a new NDSProject instance with the specified details and contexts.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="details"></param>
    /// <param name="contexts"></param>
    /// <returns></returns>
    public static Result<NDSProject> Create(string id, NDSProjectDetails details)
    {
        // Create the NDSProject instance
        var project = new NDSProject(id, details);

        // Return the created project
        return Result<NDSProject>.Success(project);
    }

    /// <summary>
    /// Appends a new context to the project.
    /// </summary>
    /// <param name="context">The context to add</param>
    /// <returns>A result containing the success of the operation.</returns>
    public Result AppendContext(NDSContext context)
    {
        // Check if a context with the same ID already exists
        if (_contexts.ContainsKey(context.Id))
        {
            // If it does, return an error
            return Result.Error($"Context with ID {context.Id} already exists in the project.");
        }

        // Add the context to the dictionary
        _contexts[context.Id] = context;

        // Return a successful result
        return Result.Success();
    }
}
