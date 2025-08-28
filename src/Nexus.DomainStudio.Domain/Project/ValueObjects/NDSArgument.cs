using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Domain.Project.ValueObjects;

/// <summary>
/// Represents an argument for an operation in Nexus Domain Studio.
/// </summary>
public sealed class NDSArgument : ValueObject
{
    /// <summary>
    /// The name of the argument.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The type of the argument.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The description of the argument.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Indicates whether the argument is required.
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// The default value of the argument, if any.
    /// </summary>
    public string? DefaultValue { get; }

    /// <summary>
    /// Private constructor to enforce the use of the Create method.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    /// <param name="isRequired"></param>
    /// <param name="defaultValue"></param>
    private NDSArgument(string name, string type, string? description, bool isRequired, string? defaultValue) 
    {
        Name = name;
        Type = type;
        Description = description;
        IsRequired = isRequired;
        DefaultValue = defaultValue;
    }

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
        // Validate the name
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSArgument>.Error("Argument name cannot be empty.");
        }

        // Validate the type
        if (string.IsNullOrWhiteSpace(type))
        {
            return Result<NDSArgument>.Error("Argument type cannot be empty.");
        }

        // Create the NDSArgument instance
        var argument = new NDSArgument(name, type, description, isRequired, defaultValue);

        // Return the argument
        return argument;
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
