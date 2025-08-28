using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an aggregate root in Nexus Domain Studio.
/// </summary>
public sealed class NDSAggregateRoot : Entity<string>, INDSContextObject
{
    /// <summary>
    /// The name of the aggregate root.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The ID of the root entity associated with this aggregate root.
    /// </summary>
    public string RootEntityId { get; private set; }

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="name"></param>
    private NDSAggregateRoot(string id, string name, string rootEntityId) : base(id)
    {
        Name = name;
        RootEntityId = rootEntityId;
    }

    /// <summary>
    /// Factory method to create a new NDSAggregateRoot instance.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Result<NDSAggregateRoot> Create(string id, string name, string rootId)
    {
        // Validate the name of the aggregate root
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSAggregateRoot>.Error("Aggregate root name cannot be empty.");
        }

        // Validate that the entity is not null
        if (string.IsNullOrWhiteSpace(rootId))
        {
            return Result<NDSAggregateRoot>.Error("Root entity ID cannot be empty.");
        }

        // Create the NDSAggregateRoot instance
        var aggregateRoot = new NDSAggregateRoot(id, name, rootId);

        // Return the created aggregate root
        return aggregateRoot;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.AggregateRoot;
}
