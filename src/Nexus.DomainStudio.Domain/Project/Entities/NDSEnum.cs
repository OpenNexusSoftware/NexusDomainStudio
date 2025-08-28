using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an enumeration in Nexus Domain Studio.
/// </summary>
public sealed class NDSEnum : Entity<string>, INDSContextObject
{
    /// <summary>
    /// The name of the enum.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The description of the enum.
    /// </summary>
    public string? Description { get; private set; }

    // Backing field for values
    private readonly List<string> _values;

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    private NDSEnum(string id, string name, string? description, List<string> values) : base(id)
    {
        Name = name;
        Description = description;
        _values = values;
    }

    /// <summary>
    /// The values of the enum.
    /// </summary>
    public IEnumerable<string> Values => _values.AsReadOnly();

    /// <summary>
    /// Creates a new NDSEnum instance with the specified name, optional description, and values.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static Result<NDSEnum> Create(string id, string name, string? description, List<string> values)
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
        var enumInstance = new NDSEnum(id, name, description, values);

        // Return the created enum
        return enumInstance;
    }

    /// <summary>
    /// Gets the type of the NDS object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Enum;
}