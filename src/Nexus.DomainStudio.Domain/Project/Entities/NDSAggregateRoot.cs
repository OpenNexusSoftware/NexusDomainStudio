using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;

namespace Nexus.DomainStudio.Domain.Project.Entities;

public class NDSAggregateRoot : Entity<string>, INDSContextObject
{
    public string Name { get; private set; }
    public string RootEntityId { get; private set; }

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="name"></param>
    private NDSAggregateRoot(string name, string rootEntityId)
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
    public Result<NDSAggregateRoot> Create(string name, string rootId)
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
        var aggregateRoot = new NDSAggregateRoot(name, rootId);

        // Return the created aggregate root
        return aggregateRoot;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.AggregateRoot;
}
