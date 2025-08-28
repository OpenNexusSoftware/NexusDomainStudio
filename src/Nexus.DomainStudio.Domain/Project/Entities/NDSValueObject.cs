using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents a value object in Nexus Domain Studio.
/// </summary>
public sealed class NDSValueObject : Entity<string>, INDSContextObject
{
    /// <summary>
    /// The properties of the value object.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The properties of the value object.
    /// </summary>
    public string Description { get; private set; }

    private List<NDSProperty> _properties;

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    private NDSValueObject(string id, string name, string description, List<NDSProperty> properties) : base(id)
    {
        Name = name;
        Description = description;
        _properties = properties;
    }

    /// <summary>
    /// The properties of the value object.
    /// </summary>
    public IEnumerable<NDSProperty> Properties => _properties.AsReadOnly();

    /// <summary>
    /// Creates a new NDSValueObject instance with the specified name, description, and properties.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Result<NDSValueObject> Create(string id, string name, string description, List<NDSProperty> properties)
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
        var valueObject = new NDSValueObject(id, name, description, properties);

        // Return the created value object
        return valueObject;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.ValueObject;
}
