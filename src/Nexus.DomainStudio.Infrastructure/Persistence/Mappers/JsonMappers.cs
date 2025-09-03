using Nexus.DomainStudio.Application.Interfaces;
using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;
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
    public static Result<NDSProject> ToEntity(this JsonProjectDTO dto)
    {
         // Get the project version and return an error result if it is invalid
        var projectVersion = NDSVersion.Create(dto.Version);
        if(!projectVersion.IsSuccess) return Result<NDSProject>.Error(projectVersion.GetErrorMessage());

        // Get the model version and return an error result if it is invalid
        var modelVersion = NDSVersion.Create(dto.ModelVersion);
        if(!modelVersion.IsSuccess) return Result<NDSProject>.Error(modelVersion.GetErrorMessage());

        // Create the project details and return an error result if it is invalid
        var projectDetails = NDSProjectDetails.Create(dto.Name, dto.Description, projectVersion.Value, modelVersion.Value);
        if(!projectDetails.IsSuccess) return Result<NDSProject>.Error(projectDetails.GetErrorMessage());

        // Create the project
        var projectResult = NDSProject.Create(Guid.NewGuid().ToString(), projectDetails.Value);
        if(!projectResult.IsSuccess) return Result<NDSProject>.Error(projectResult.GetErrorMessage());

        // Get the project from the result and return the value
        var project = projectResult.Value;
        return project;
    }

    /// <summary>
    /// Converts a JsonContextDTO to an NDSContext entity.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="contextObjects"></param>
    /// <returns></returns>
    public static Result<NDSContext> ToEntity(this JsonContextDTO ndsContextDto)
    {
        // Get the context version and return an error result if it is invalid
        var version = NDSVersion.Create(ndsContextDto.Version);
        if (!version.IsSuccess) return Result<NDSContext>.Error(version.GetErrorMessage());

        // Create the context details and return an error result if it is invalid
        var details = NDSContextDetails.Create(ndsContextDto.Name, ndsContextDto.Description, version.Value);
        if (!details.IsSuccess) return Result<NDSContext>.Error(details.GetErrorMessage());

        // Create the context and return the result
        var context = NDSContext.Create(ndsContextDto.Id, details.Value);
        return context;
    }

    /// <summary>
    /// Converts a collection of JsonAggregateDTOs to a collection of NDSAggregateRoot entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSAggregateRoot> ToEntity(this JsonAggregateDTO dto)
    {
        var aggregateResult = NDSAggregateRoot.Create(dto.Id, dto.Name, dto.Root);
        return aggregateResult;
    }

    /// <summary>
    /// Converts a collection of JsonEntityDTOs to a collection of NDSEntity entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSEntity> ToEntity(this JsonEntityDTO dto)
    {
        // Convert properties
        var propertyConversion = dto.Properties.Select(o => o.ToValueObject()).ToList();

        // Check for conversion errors
        if (propertyConversion.Any(r => !r.IsSuccess))
        {
            // Aggregate error messages
            var errors = string.Join("; ", propertyConversion.Where(r => !r.IsSuccess).Select(r => r.GetErrorMessage()));
            return Result<NDSEntity>.Error($"Failed to convert properties: {errors}");
        }

        // Create the entity and return the result
        var entityResult = NDSEntity.Create(dto.Id, dto.Name, dto.Description, propertyConversion.Select(o => o.Value).ToList(), dto.Operations.ToList());
        return entityResult;
    }

    /// <summary>
    /// Converts a JsonPropertyDTO to an NDSProperty value object.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static Result<NDSProperty> ToValueObject(this JsonPropertyDTO dto)
    {
        var propertyResult = NDSProperty.Create(dto.Name, dto.Type, true, dto.Description);
        return propertyResult;
    }

    /// <summary>
    /// Converts a collection of JsonEnumDTOs to a collection of NDSEnum entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSValueObject> ToEntity(this JsonValueObjectDTO dto)
    {
        // Convert properties
        var propertyConversion = dto.Properties.Select(o => o.ToValueObject()).ToList();

        // Check for conversion errors
        if (propertyConversion.Any(r => !r.IsSuccess))
        {
            // Aggregate error messages
            var errors = string.Join("; ", propertyConversion.Where(r => !r.IsSuccess).Select(r => r.GetErrorMessage()));
            return Result<NDSValueObject>.Error($"Failed to convert properties: {errors}");
        }

        // Create and return the result
        return NDSValueObject.Create(dto.Id, dto.Name, dto.Description, propertyConversion.Select(o => o.Value).ToList());
    }

    /// <summary>
    /// Converts a collection of JsonEnumDTOs to a collection of NDSEnum entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSEnum> ToEntity(this JsonEnumDTO dto)
    {
        // Create and return the result
        var ndsEnum = NDSEnum.Create(dto.Id, dto.Name, dto.Description, dto.Values.ToList());
        return ndsEnum;
    }

    /// <summary>
    /// Converts a collection of JsonEventDTOs to a collection of NDSEvent entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSEvent> ToEntity(this JsonEventDTO dto)
    {
        // Convert properties
        var propertyConversion = dto.Properties.Select(o => o.ToValueObject()).ToList();

        // Check for conversion errors
        if (propertyConversion.Any(r => !r.IsSuccess))
        {
            // Aggregate error messages
            var errors = string.Join("; ", propertyConversion.Where(r => !r.IsSuccess).Select(r => r.GetErrorMessage()));
            return Result<NDSEvent>.Error($"Failed to convert properties: {errors}");
        }

        var ndsEvent = NDSEvent.Create(dto.Id, dto.Name, dto.Description, propertyConversion.Select(o => o.Value).ToList());
        return ndsEvent;
    }

    /// <summary>
    /// Converts a collection of JsonOperationDTOs to a collection of NDSOperation entities.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public static Result<NDSOperation> ToEntity(this JsonOperationDTO dto)
    {
        var ndsOperation = NDSOperation.Create(dto.Id, dto.Name, dto.Description, [], []);
        return ndsOperation;
    }
}
