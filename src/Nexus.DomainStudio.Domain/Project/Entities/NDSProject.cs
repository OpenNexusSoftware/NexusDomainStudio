using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents a project in Nexus Domain Studio.
/// </summary>
public class NDSProject : AggregateRoot<string>
{
    public NDSProjectDetails Details { get; private set; }
    public Dictionary<string, NDSContext> Contexts { get; private set; }

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="details"></param>
    /// <param name="contexts"></param>
    private NDSProject(NDSProjectDetails details, Dictionary<string, NDSContext> contexts)
    {
        Details = details;
        Contexts = contexts;
    }

    public static Result<NDSProject> Create(NDSProjectDetails details, IEnumerable<NDSContext> contexts)
    {
        // Map contexts to a dictionary for easy access
        var contextDict = contexts.ToDictionary(c => c.Id, c => c);

        // Create the NDSProject instance
        var project = new NDSProject(details, contextDict);

        // Return the created project
        return Result<NDSProject>.Success(project);
    }
}
