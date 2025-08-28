using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

public class NDSProperty : ValueObject
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required bool IsRequired { get; init; }
    public string? Description { get; init; }
    public string? DefaultValue { get; init; }

    /// <summary>
    /// Private constructor to enforce the use of the Create method.
    /// </summary>
    private NDSProperty() { }

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
        var property = new NDSProperty
        {
            Name = name,
            Type = type,
            IsRequired = isRequired,
            Description = description,
            DefaultValue = defaultValue
        };

        // Return the property
        return property;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Type;
        yield return Description;
        yield return IsRequired;
        yield return DefaultValue;
    }
}
