using Nexus.DomainStudio.Application.Interfaces;
using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Infrastructure.Persistence;

/// <summary>
/// Handles deserialization of NDSContext from JSON format.
/// </summary>
public class JsonNDSContextDeserializer : INDSContextDeserializer
{
    public JsonNDSContextDeserializer()
    {

    }

    public Result<NDSContext> Deserialize(string source)
    {
        // Deserialize the JSON file into a DTO
        var result = JsonHelper.DeserializeFromFile<JsonContextDTO>(source);

        if(!result.IsSuccess) return Result<NDSContext>.Error(result.GetErrorMessage());

        // Convert the DTO to the domain entity

        return Result<NDSContext>.Error("Not implemented");
    }
}