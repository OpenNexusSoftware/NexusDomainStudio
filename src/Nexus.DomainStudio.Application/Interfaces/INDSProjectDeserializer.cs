using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;
using Nexus.DomainStudio.Domain.Project.Interfaces;

namespace Nexus.DomainStudio.Application.Interfaces;

/// <summary>
/// Result of deserializing a JSON representation of an NDSProject.
/// </summary>
public class JsonNDSProjectDeserializedResult
{
    public NDSProject Project { get; init; }
    public List<Result> DeserialiseErrors { get; private set; }
    public List<Result<INDSContextObject>> ConversionErrors { get; private set; }

    public JsonNDSProjectDeserializedResult(NDSProject project)
    {
        Project = project;
        DeserialiseErrors = [];
        ConversionErrors = [];
    }
}

public interface INDSProjectDeserializer
{
    /// <summary>
    /// Deserializes the given source string into an NDSProject object.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Result<JsonNDSProjectDeserializedResult> Deserialize(string source);
}
