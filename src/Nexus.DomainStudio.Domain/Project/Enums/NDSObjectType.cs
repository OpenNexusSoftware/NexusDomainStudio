namespace Nexus.DomainStudio.Domain.Project.Enums;

/// <summary>
/// Enumeration of different types of NDS (Nexus Domain Studio) objects.
/// </summary>
public enum NDSObjectType
{
    /// <summary>
    /// Represents an aggregate root in the domain model.
    /// </summary>
    AggregateRoot,

    /// <summary>
    /// Represents an entity in the domain model.
    /// </summary>
    Entity,

    /// <summary>
    /// Represents a value object in the domain model.
    /// </summary>
    Enum,

    /// <summary>
    /// Represents a domain event in the domain model.
    /// </summary>
    Event,

    /// <summary>
    /// Represents an operation in the domain model.
    /// </summary>
    Operation,

    /// <summary>
    /// Represents a value object in the domain model.
    /// </summary>
    ValueObject,
}
