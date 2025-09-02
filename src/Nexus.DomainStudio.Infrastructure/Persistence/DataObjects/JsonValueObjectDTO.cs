using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing a Value Object in JSON format.
/// </summary>
public class JsonValueObjectDTO
{
    /// <summary>
    /// Unique identifier for the Value Object.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the Value Object.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the Value Object.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of properties associated with the Value Object.
    /// </summary>
    [JsonPropertyName("properties")]
    public IEnumerable<JsonPropertyDTO> Properties { get; set; } = [];
}