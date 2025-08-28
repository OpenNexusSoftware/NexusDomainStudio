using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents a context within a Nexus Domain Studio project.
/// </summary>
public sealed class NDSContext : Entity<string>
{
    /// <summary>
    /// Details about the context.
    /// </summary>
    public NDSContextDetails Details { get; private set; }

    // // Internal dictionaries to hold context objects
    // private readonly Dictionary<string, NDSAggregateRoot> _aggregateRoots;
    // private readonly Dictionary<string, NDSEntity> _entities;
    // private readonly Dictionary<string, NDSEnum> _enums;
    // private readonly Dictionary<string, NDSEvent> _events;
    // private readonly Dictionary<string, NDSOperation> _operations;
    // private readonly Dictionary<string, NDSValueObject> _valueObjects;

    // Dictionary to hold all context objects
    private readonly Dictionary<string, INDSContextObject> _symbols;

    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// </summary>
    /// <param name="details"></param>
    private NDSContext(
        string id,
        NDSContextDetails details
    ) : base(id)
    {
        Details = details;
        _symbols = new Dictionary<string, INDSContextObject>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Read-only access to the aggregate roots in the context.
    /// </summary>
    public IEnumerable<NDSAggregateRoot> AggregateRoots => _symbols.Values.OfType<NDSAggregateRoot>();

    /// <summary>
    /// Read-only access to the entities in the context.
    /// </summary>
    public IEnumerable<NDSEntity> Entities => _symbols.Values.OfType<NDSEntity>();

    /// <summary>
    /// Read-only access to the enums in the context.
    /// </summary>
    public IEnumerable<NDSEnum> Enums => _symbols.Values.OfType<NDSEnum>();

    /// <summary>
    /// Read-only access to the events in the context.
    /// </summary>
    public IEnumerable<NDSEvent> Events => _symbols.Values.OfType<NDSEvent>();

    /// <summary>
    /// Read-only access to the operations in the context.
    /// </summary>
    public IEnumerable<NDSOperation> Operations => _symbols.Values.OfType<NDSOperation>();

    /// <summary>
    /// Read-only access to the value objects in the context.
    /// </summary>
    public IEnumerable<NDSValueObject> ValueObjects => _symbols.Values.OfType<NDSValueObject>();

    /// <summary>
    /// Factory method to create a new NDSContext instance.
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static NDSContext Create(string id, NDSContextDetails details)
    {
        // Add any necessary validation here
        return new NDSContext(id, details);
    }

    /// <summary>
    /// Adds an aggregate root to the context.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="symbol"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public Result AddSymbol(string id, INDSContextObject symbol)
    {
        // Check if the symbol ID already exists
        if (_symbols.ContainsKey(id))
        {
            return Result.Error($"A symbol with the ID '{id}' already exists in the context.");
        }

        // Add the symbol to the appropriate dictionary
        _symbols[id] = symbol;

        // Return success
        return Result.Success();
    }
}
