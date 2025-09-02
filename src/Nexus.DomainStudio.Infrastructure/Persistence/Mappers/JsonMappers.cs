using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;

namespace Nexus.DomainStudio.Infrastructure.Persistence.Mappers;

public static class JsonMappers
{
    /// <summary>
    /// Converts a JsonProjectDTO to an NDSProject entity.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="contexts"></param>
    /// <returns></returns>
    public static Result<NDSProject> ToEntity(this JsonProjectDTO dto, List<NDSContext> contexts)
    {
        return Result<NDSProject>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a JsonContextDTO to an NDSContext entity.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="contextObjects"></param>
    /// <returns></returns>
    public static Result<NDSContext> ToEntity(this JsonContextDTO dto, IEnumerable<INDSContextObject> contextObjects)
    {
        return Result<NDSContext>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a collection of JsonAggregateDTOs to a collection of NDSAggregateRoot entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSAggregateRoot> ToEntity(this JsonAggregateDTO dto)
    {
        return Result<NDSAggregateRoot>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a collection of JsonEntityDTOs to a collection of NDSEntity entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSEntity> ToEntity(this JsonEntityDTO dto)
    {
        return Result<NDSEntity>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a collection of JsonEnumDTOs to a collection of NDSEnum entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSValueObject> ToEntity(this JsonValueObjectDTO dto)
    {
        return Result<NDSValueObject>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a collection of JsonEnumDTOs to a collection of NDSEnum entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSEnum> ToEntity(this JsonEnumDTO dto)
    {
        return Result<NDSEnum>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a collection of JsonEventDTOs to a collection of NDSEvent entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSEvent> ToEntity(this JsonEventDTO dto)
    {
        return Result<NDSEvent>.Error("Not implemented");
    }

    /// <summary>
    /// Converts a collection of JsonOperationDTOs to a collection of NDSOperation entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSOperation> ToEntity(this JsonOperationDTO dto)
    {
        return Result<NDSOperation>.Error("Not implemented");
    }
}
