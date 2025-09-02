using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing a property in JSON format.
/// </summary>
public class JsonPropertyDTO
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    [JsonPropertyName("id")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Type of the property.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Description of the property.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
