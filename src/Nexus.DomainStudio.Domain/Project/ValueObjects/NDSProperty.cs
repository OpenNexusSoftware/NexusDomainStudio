using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

/// <summary>
/// Represents a property in Nexus Domain Studio.
/// </summary>
public sealed class NDSProperty : ValueObject
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The type of the property.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Indicates whether the property is required.
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// The description of the property.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// The default value of the property, if any.
    /// </summary>
    public string? DefaultValue { get; }

    /// <summary>
    /// Private constructor to enforce the use of the Create method.
    /// </summary>
    private NDSProperty(string name, string type, bool isRequired, string? description, string? defaultValue)
    {
        Name = name;
        Type = type;
        IsRequired = isRequired;
        Description = description;
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Creates a new NDSProperty instance after validating the inputs.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="isRequired"></param>
    /// <param name="description"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Result<NDSProperty> Create(string name, string type, bool isRequired, string? description = null, string? defaultValue = null)
    {
        // Check if the name is null or empty
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSProperty>.Error("Property name cannot be empty.");
        }

        // Check if the type is null or empty
        if (string.IsNullOrWhiteSpace(type))
        {
            return Result<NDSProperty>.Error("Property type cannot be empty.");
        }

        // Create the NDSProperty instance
        var property = new NDSProperty(name, type, isRequired, description, defaultValue);

        // Return the property
        return property;
    }

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        // Return the properties that define equality
        yield return Name;
        yield return Type;
        yield return Description;
        yield return IsRequired;
        yield return DefaultValue;
    }
}
