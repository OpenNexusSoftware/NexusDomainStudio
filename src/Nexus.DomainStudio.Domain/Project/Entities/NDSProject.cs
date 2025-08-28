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
    public readonly Dictionary<string, NDSContext> _contexts;

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="details"></param>
    /// <param name="contexts"></param>
    private NDSProject(string id, NDSProjectDetails details, Dictionary<string, NDSContext> contexts) : base(id)
    {
        Details = details;
        _contexts = contexts;
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
    public static Result<NDSProject> Create(string id, NDSProjectDetails details, IEnumerable<NDSContext> contexts)
    {
        // Map contexts to a dictionary for easy access
        var contextDict = contexts.ToDictionary(c => c.Id, c => c);

        // Create the NDSProject instance
        var project = new NDSProject(id, details, contextDict);

        // Return the created project
        return Result<NDSProject>.Success(project);
    }
}
