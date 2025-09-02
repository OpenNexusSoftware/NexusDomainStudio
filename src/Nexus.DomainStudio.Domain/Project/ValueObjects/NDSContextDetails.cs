using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

/// <summary>
/// Represents the details of a Nexus Domain Studio context.
/// </summary>
public sealed class NDSContextDetails : ValueObject
{
    /// <summary>
    /// The name of the context.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The description of the context.
    /// </summary>
    public string Description { get;}

    /// <summary>
    /// The version of the context.
    /// </summary>
    public NDSVersion Version { get; }

    /// <summary>
    /// Private constructor to enforce the use of the factory method.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="version"></param>
    private NDSContextDetails(string name, string description, NDSVersion version) 
    {
        Name = name;
        Description = description;
        Version = version;
    }

    /// <summary>
    /// Factory method to create a new instance of NDSContextDetails.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public static Result<NDSContextDetails> Create(string name, string description, NDSVersion version)
    {
        // Add any necessary validation here
        return new NDSContextDetails(name, description, version);
    }

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        // Return the properties that define equality
        yield return Name;
        yield return Description;
        yield return Version;
    }
}