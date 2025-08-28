using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

/// <summary>
/// Represents an argument for an operation in Nexus Domain Studio.
/// </summary>
public class NDSArgument : ValueObject
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? Description { get; set; }
    public required bool IsRequired { get; set; }
    public string? DefaultValue { get; set; }

    private NDSArgument() { }

    /// <summary>
    /// Creates a new instance of <see cref="NDSArgument"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="isRequired"></param>
    /// <param name="description"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Result<NDSArgument> Create(string name, string type, bool isRequired, string? description = null, string? defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<NDSArgument>.Error("Argument name cannot be empty.");

        if (string.IsNullOrWhiteSpace(type))
            return Result<NDSArgument>.Error("Argument type cannot be empty.");

        var argument = new NDSArgument
        {
            Name = name,
            Type = type,
            IsRequired = isRequired,
            Description = description,
            DefaultValue = defaultValue
        };

        return Result<NDSArgument>.Success(argument);
    }

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Type;
        yield return Description;
        yield return IsRequired;
        yield return DefaultValue;
    }
}
