using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing a Project in JSON format.
/// </summary>
public class JsonProjectDTO
{
    /// <summary>
    /// Unique identifier for the Project.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Version of the Project.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Version of the model the project adheres to.
    /// </summary>
    [JsonPropertyName("modelVersion")]
    public string ModelVersion { get; set; } = string.Empty;

    /// <summary>
    /// Description of the Project.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of contexts associated with the Project.
    /// </summary>
    [JsonPropertyName("contexts")]
    public string[] Contexts { get; set; } = [];
}
