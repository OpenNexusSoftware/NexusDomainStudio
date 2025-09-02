using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object representing the sources of a JSON context.
/// </summary>
public class JsonSourcesDTO
{
    /// <summary>
    /// List of aggregate identifiers in the context.
    /// </summary>
    [JsonPropertyName("aggregates")]
    public string[] Aggregates { get; set; } = [];

    /// <summary>
    /// List of entity identifiers in the context.
    /// </summary>
    [JsonPropertyName("entities")]
    public string[] Entities { get; set; } = [];

    /// <summary>
    /// List of enum identifiers in the context.
    /// </summary>
    [JsonPropertyName("enums")]
    public string[] Enums { get; set; } = [];

    /// <summary>
    /// List of event identifiers in the context.
    /// </summary>
    [JsonPropertyName("events")]
    public string[] Events { get; set; } = [];

    /// <summary>
    /// List of operation identifiers in the context.
    /// </summary>
    [JsonPropertyName("operations")]
    public string[] Operations { get; set; } = [];

    /// <summary>
    /// List of value object identifiers in the context.
    /// </summary>
    [JsonPropertyName("valueObjects")]
    public string[] ValueObjects { get; set; } = [];
}

/// <summary>
/// Data Transfer Object representing a JSON context.
/// </summary>
public class JsonContextDTO
{
    /// <summary>
    /// Unique identifier for the context.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the context.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the context.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Version of the context.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
    
    /// <summary>
    /// Sources associated with the context.
    /// </summary>
    [JsonPropertyName("sources")]
    public JsonSourcesDTO Sources { get; set; } = new();
}