using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing an Argument in JSON format.
/// </summary>
public class JsonArgumentDTO
{
    /// <summary>
    /// Unique identifier for the Argument.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Data type of the Argument.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the Argument is required. Defaults to true.
    /// </summary>
    [JsonPropertyName("required")]
    public bool Required { get; set; } = true;

    /// <summary>
    /// Description of the Argument.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
