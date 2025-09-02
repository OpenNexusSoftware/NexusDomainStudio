using System.Text.Json.Serialization;

namespace Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

/// <summary>
/// Data Transfer Object (DTO) representing a property mapping in JSON format.
/// </summary>
public class JsonPropertyMappingDTO
{
    /// <summary>
    /// The property name to be mapped.
    /// </summary>
    [JsonPropertyName("property")]
    public string Property { get; set; } = string.Empty;

    /// <summary>
    /// The value to which the property is mapped.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object (DTO) representing an Operation Emit in JSON format.
/// </summary>
public class JsonOperationEmitDTO
{
    /// <summary>
    /// The identifier of the event that this operation emits.
    /// </summary>
    [JsonPropertyName("event")]
    public string EventId { get; set; } = string.Empty;

    /// <summary>
    /// A description of the emit action.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of property mappings for the emitted event.
    /// </summary>
    [JsonPropertyName("maps")]
    public IEnumerable<JsonPropertyMappingDTO> Mappings { get; set; } = [];
}

/// <summary>
/// Data Transfer Object (DTO) representing an Operation in JSON format.
/// </summary>
public class JsonOperationDTO
{
    /// <summary>
    /// Unique identifier for the operation.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the operation.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the operation.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of events that this operation emits.
    /// </summary>
    [JsonPropertyName("emits")]
    public IEnumerable<JsonOperationEmitDTO> Emits { get; set; } = [];
}
