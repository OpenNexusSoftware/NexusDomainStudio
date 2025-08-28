using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents a context within a Nexus Domain Studio project.
/// </summary>
public class NDSContext : Entity<string>
{
    /// <summary>
    /// Details about the context.
    /// </summary>
    public NDSContextDetails Details { get; private set; }

    public Dictionary<string, NDSAggregateRoot> AggregateRoots { get; private set; }
    public Dictionary<string, NDSEntity> Entities { get; private set; }
    public Dictionary<string, NDSEnum> Enums { get; private set; }
    public Dictionary<string, NDSEvent> Events { get; private set; }
    public Dictionary<string, NDSOperation> Operations { get; private set; }
    public Dictionary<string, NDSValueObject> ValueObjects { get; private set; }

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="details"></param>
    private NDSContext(
        NDSContextDetails details,
        Dictionary<string, NDSAggregateRoot>? aggregateRoots = null,
        Dictionary<string, NDSEntity>? entities = null,
        Dictionary<string, NDSEnum>? enums = null,
        Dictionary<string, NDSEvent>? events = null,
        Dictionary<string, NDSOperation>? operations = null,
        Dictionary<string, NDSValueObject>? valueObjects = null
    )
    {
        Details = details;
        AggregateRoots = aggregateRoots ?? [];
        Entities = entities ?? [];
        Enums = enums ?? [];
        Events = events ?? [];
        Operations = operations ?? [];
        ValueObjects = valueObjects ?? [];
    }

    /// <summary>
    /// Factory method to create a new NDSContext instance.
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static NDSContext Create(NDSContextDetails details)
    {
        // Add any necessary validation here
        return new NDSContext(details);
    }
}
