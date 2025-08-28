using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an entity in Nexus Domain Studio.
/// </summary>
public sealed class NDSEntity : Entity<string>, INDSContextObject
{
    /// <summary>
    /// The name of the entity.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The description of the entity.
    /// </summary>
    public string? Description { get; private set; }

    // Backing fields
    private readonly List<NDSProperty> _properties;
    private readonly List<string> _operationIds;

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    /// <param name="operationIds"></param>
    private NDSEntity(string id, string name, string? description, List<NDSProperty> properties, List<string> operationIds) : base(id)
    {
        Name = name;
        Description = description;
        _properties = properties;
        _operationIds = operationIds;
    }

    /// <summary>
    /// The properties of the entity.
    /// </summary>
    public IEnumerable<NDSProperty> Properties => _properties.AsReadOnly();

    /// <summary>
    /// The operation IDs associated with the entity.
    /// </summary>
    public IEnumerable<string> OperationIds => _operationIds.AsReadOnly();

    /// <summary>
    /// Creates a new NDSEntity instance with the specified name, optional description, properties, and operation IDs.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    /// <param name="operationIds"></param>
    /// <returns></returns>
    public static Result<NDSEntity> Create(string id, string name, string description, List<NDSProperty> properties, List<string> operationIds)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSEntity>.Error("Name cannot be null or empty.");
        }

        // Create the NDSEntity instance
        var entity = new NDSEntity(id, name, description, properties, operationIds);

        // Return the created entity
        return entity;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Entity;
}
