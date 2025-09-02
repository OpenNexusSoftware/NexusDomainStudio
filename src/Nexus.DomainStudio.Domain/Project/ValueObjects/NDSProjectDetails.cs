using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

/// <summary>
/// Represents the details of a Nexus Domain Studio project.
/// </summary>
public sealed class NDSProjectDetails : ValueObject
{
    /// <summary>
    /// The name of the project.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The description of the project.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// The version of the project.
    /// </summary>
    public NDSVersion Version { get; }

    /// <summary>
    /// The version of the underlying model the project is based on.
    /// </summary>
    public NDSVersion ModelVersion { get; }

    /// <summary>
    /// Private constructor to enforce the use of the factory method.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="version"></param>
    /// <param name="modelVersion"></param>
    private NDSProjectDetails(string name, string? description, NDSVersion version, NDSVersion modelVersion)
    {
        Name = name;
        Description = description;
        Version = version;
        ModelVersion = modelVersion;
    }

    /// <summary>
    /// Factory method to create a new instance of NDSProjectDetails.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="version"></param>
    /// <param name="modelVersion"></param>
    /// <returns></returns>
    public static Result<NDSProjectDetails> Create(string name, string? description, NDSVersion version, NDSVersion modelVersion)
    {
        // Add any necessary validation here
        return new NDSProjectDetails(name, description, version, modelVersion);
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
        yield return ModelVersion;
    }
}