using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an event in Nexus Domain Studio.
/// </summary>
public sealed class NDSEvent : Entity<string>, INDSContextObject
{
    /// <summary>
    /// The name of the event.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The description of the event.
    /// </summary>
    public string? Description { get; private set; }
    
    // Backing field for properties
    private readonly List<NDSProperty> _properties;

    // Private constructor to enforce the use of the factory method
    private NDSEvent(string id, string name, string? description, List<NDSProperty> properties) : base(id)
    {
        Name = name;
        Description = description;
        _properties = properties;
    }

    /// <summary>
    /// The properties of the event.
    /// </summary>
    public IEnumerable<NDSProperty> Properties => _properties.AsReadOnly();

    /// <summary>
    /// Creates a new NDSEvent instance with the specified name and optional description.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static Result<NDSEvent> Create(string id, string name, string? description, List<NDSProperty> properties)
    {
        // Validate the name of the event
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSEvent>.Error("Event name cannot be empty.");
        }

        // Create the NDSEvent instance
        var ndsEvent = new NDSEvent(id, name, description, properties);

        // Return the created event
        return ndsEvent;
    }

    /// <summary>
    /// Gets the type of the context object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Event;
}
