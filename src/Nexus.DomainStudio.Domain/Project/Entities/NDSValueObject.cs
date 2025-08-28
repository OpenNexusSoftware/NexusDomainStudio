using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents a value object in Nexus Domain Studio.
/// </summary>
public class NDSValueObject : Entity<string>, INDSContextObject
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<NDSProperty> Properties { get; set; }

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    private NDSValueObject(string name, string description, List<NDSProperty> properties)
    {
        Name = name;
        Description = description;
        Properties = properties;
    }

    /// <summary>
    /// Creates a new NDSValueObject instance with the specified name, description, and properties.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Result<NDSValueObject> Create(string name, string description, List<NDSProperty> properties)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSValueObject>.Error("Name cannot be null or empty.");
        }

        // Validate if the inputs are not null or empty
        if (properties == null || properties.Count == 0)
        {
            return Result<NDSValueObject>.Error("Properties cannot be null or empty.");
        }

        // Create the NDSValueObject instance
        var valueObject = new NDSValueObject(name, description, properties);

        // Return the created value object
        return valueObject;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.ValueObject;
}
