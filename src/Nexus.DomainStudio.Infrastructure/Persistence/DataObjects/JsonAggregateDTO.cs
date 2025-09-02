using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing an Aggregate in JSON format.
/// </summary>
public class JsonAggregateDTO
{
    /// <summary>
    /// Unique identifier for the Aggregate.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the Aggregate.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the Aggregate.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Root entity of the Aggregate.
    /// </summary>
    [JsonPropertyName("root")]
    public string Root { get; set; } = string.Empty;
}