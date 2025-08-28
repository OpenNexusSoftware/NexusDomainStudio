using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

public class NDSOperationEmit : ValueObject
{
    public string EventId { get; private set; }
    public string? Description { get; private set; }
    public IEnumerable<NDSMapping> Mappings { get; set; }

    /// <summary>
    /// Private constructor to enforce the use of the factory method.
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="description"></param>
    /// <param name="mappings"></param>
    private NDSOperationEmit(string eventId, string description, IEnumerable<NDSMapping> mappings) 
    {
        EventId = eventId;
        Description = description;
        Mappings = mappings;
    }

    /// <summary>
    /// Factory method to create a new instance of NDSOperationEmit.
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="description"></param>
    /// <param name="mappings"></param>
    /// <returns></returns>
    public static NDSOperationEmit Create(string eventId, string description, IEnumerable<NDSMapping> mappings)
    {
        // Add any necessary validation logic here
        return new NDSOperationEmit(eventId, description, mappings);
    }

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EventId;
        yield return Description;
        foreach (var mapping in Mappings)
        {
            yield return mapping;
        }
    }
}