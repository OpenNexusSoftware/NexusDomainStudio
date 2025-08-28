using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an enumeration in Nexus Domain Studio.
/// </summary>
public class NDSEnum : Entity<string>, INDSContextObject
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public List<string> Values { get; private set; }

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    private NDSEnum(string name, string? description, List<string> values)
    {
        Name = name;
        Description = description;
        Values = values;
    }

    /// <summary>
    /// Creates a new NDSEnum instance with the specified name, optional description, and values.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static Result<NDSEnum> Create(string name, string? description, List<string> values)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSEnum>.Error("Name cannot be null or empty.");
        }

        if (values == null || values.Count == 0)
        {
            return Result<NDSEnum>.Error("Values cannot be null or empty.");
        }

        // Create the NDSEnum instance
        var enumInstance = new NDSEnum(name, description, values);

        // Return the created enum
        return enumInstance;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Enum;
}