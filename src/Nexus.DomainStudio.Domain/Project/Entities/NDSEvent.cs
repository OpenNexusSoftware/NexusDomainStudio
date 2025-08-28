using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

public class NDSEvent : Entity<string>, INDSContextObject
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    
    public List<NDSProperty> Properties { get; private set; } = [];

    // Private constructor to enforce the use of the factory method
    private NDSEvent(string name, string? description, List<NDSProperty> properties) 
    {
        Name = name;
        Description = description;
        Properties = properties;
    }

    /// <summary>
    /// Creates a new NDSEvent instance with the specified name and optional description.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static Result<NDSEvent> Create(string name, string? description, List<NDSProperty> properties)
    {
        // Validate the name of the event
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSEvent>.Error("Event name cannot be empty.");
        }

        // Create the NDSEvent instance
        var ndsEvent = new NDSEvent(name, description, properties);

        // Return the created event
        return ndsEvent;
    }

    /// <summary>
    /// Gets the type of the context object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Event;
}
