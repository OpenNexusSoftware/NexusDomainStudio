using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an entity in Nexus Domain Studio.
/// </summary>
public class NDSEntity : Entity<string>, INDSContextObject
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public List<NDSProperty> Properties { get; private set; } = [];
    public List<string> OperationIds { get; private set; } = [];

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    /// <param name="operationIds"></param>
    private NDSEntity(string name, string? description, List<NDSProperty> properties, List<string> operationIds)
    {
        Name = name;
        Description = description;
        Properties = properties;
        OperationIds = operationIds;
    }

    /// <summary>
    /// Creates a new NDSEntity instance with the specified name, optional description, properties, and operation IDs.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="properties"></param>
    /// <param name="operationIds"></param>
    /// <returns></returns>
    public static Result<NDSEntity> Create(string name, string description, List<NDSProperty> properties, List<string> operationIds)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSEntity>.Error("Name cannot be null or empty.");
        }

        // Create the NDSEntity instance
        var entity = new NDSEntity(name, description, properties, operationIds);

        // Return the created entity
        return entity;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Entity;
}
