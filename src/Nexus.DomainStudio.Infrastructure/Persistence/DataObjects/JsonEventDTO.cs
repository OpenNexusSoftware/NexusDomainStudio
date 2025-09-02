using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing an Event in JSON format.
/// </summary>
public class JsonEventDTO
{
    /// <summary>
    /// Unique identifier for the Event.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the Event.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the Event.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of properties associated with the Event.
    /// </summary>
    [JsonPropertyName("properties")]
    public IEnumerable<JsonPropertyDTO> Properties { get; set; } = [];
}