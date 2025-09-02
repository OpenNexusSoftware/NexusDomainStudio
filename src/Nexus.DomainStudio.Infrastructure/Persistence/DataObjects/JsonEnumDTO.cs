using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

public class JsonEnumDTO
{
    /// <summary>
    /// Unique identifier for the enum.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the enum.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the enum.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of values in the enum.
    /// </summary>
    [JsonPropertyName("values")]
    public IEnumerable<string> Values { get; set; } = [];
}
