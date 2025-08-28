using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

/// <summary>
/// Represents a mapping of a property to a value within a Nexus Domain Studio project.
/// </summary>
public sealed class NDSMapping : ValueObject
{
    /// <summary>
    /// The property being mapped.
    /// </summary>
    public string Property { get; }

    /// <summary>
    /// The value associated with the property.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Private constructor for ORM and serialization purposes.
    /// </summary>
    private NDSMapping(string property, string value) 
    {
        Property = property;
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="NDSMapping"/> with the specified property and value.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<NDSMapping> Create(string property, string value)
    {
        // Check if the property is null or empty
        if (string.IsNullOrWhiteSpace(property))
        {
            return Result<NDSMapping>.Error("Property cannot be null or empty.");
        }

        // Check if the value is null or empty
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<NDSMapping>.Error("Value cannot be null or empty.");
        }

        // Create the NDSMapping instance
        var mapping = new NDSMapping(property, value);

        // Return the mapping
        return mapping;
    }

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        // Return the property and value for equality comparison
        yield return Property;
        yield return Value;
    }
}