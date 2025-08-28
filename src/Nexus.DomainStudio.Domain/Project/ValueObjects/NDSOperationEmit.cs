using System.Collections.Immutable;
using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

public sealed class NDSOperationEmit : ValueObject
{
    /// <summary>
    /// The event identifier for the emit.
    /// </summary>
    public string EventId { get; }

    /// <summary>
    /// The description of the emit.
    /// </summary>
    public string? Description { get; }

    // The mappings for the event emit.
    private readonly ImmutableArray<NDSMapping> _mappings;

    /// <summary>
    /// Private constructor to enforce the use of the factory method.
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="description"></param>
    /// <param name="mappings"></param>
    private NDSOperationEmit(string eventId, string? description, ImmutableArray<NDSMapping> mappings) 
    {
        EventId = eventId;
        Description = description;
        _mappings = mappings;
    }

    /// <summary>
    /// The mappings for the event emit.
    /// </summary>
    public IEnumerable<NDSMapping> Mappings => _mappings;

    /// <summary>
    /// Factory method to create a new instance of NDSOperationEmit.
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="description"></param>
    /// <param name="mappings"></param>
    /// <returns></returns>
    public static NDSOperationEmit Create(string eventId, string? description, IEnumerable<NDSMapping> mappings)
    {
        // Add any necessary validation logic here
        return new NDSOperationEmit(eventId, description, mappings.ToImmutableArray());
    }

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return Description;
        foreach (var mapping in _mappings) yield return mapping;
    }
}