using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing an Entity in JSON format.
/// </summary>
public class JsonEntityDTO
{
    /// <summary>
    /// Unique identifier for the Entity.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the Entity.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the Entity.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of properties associated with the Entity.
    /// </summary>
    [JsonPropertyName("properties")]
    public IEnumerable<JsonPropertyDTO> Properties { get; set; } = [];

    /// <summary>
    /// List of operations associated with the Entity.
    /// </summary>
    [JsonPropertyName("operations")]
    public IEnumerable<string> Operations { get; set; } = [];
}
