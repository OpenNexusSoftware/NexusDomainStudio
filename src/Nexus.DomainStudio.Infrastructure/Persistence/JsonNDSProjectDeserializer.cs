using Microsoft.Extensions.Logging;
using Nexus.DomainStudio.Application.Interfaces;
using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Persistence.Mappers;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Infrastructure.Persistence;

/// <summary>
/// Concrete implementation of INDSProjectDeserializer that deserializes NDSProject from JSON.
/// </summary>
public class JsonNDSProjectDeserializer : INDSProjectDeserializer
{
    private readonly ILogger<JsonNDSProjectDeserializer> _logger;

    public JsonNDSProjectDeserializer(ILogger<JsonNDSProjectDeserializer> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Loads NDS objects of type T from the given sources.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sources"></param>
    /// <returns></returns>
    private List<T> LoadNDSObjectsFromSources<T>(string basePath, IEnumerable<string> sources) where T : class
    {
        // Get all the aggregates from the context DTO
        var sourceFiles = new HashSet<string>();
        foreach (var objectSource in sources)
        {
            // Get all the glob files matching the aggregate source pattern and add them to the list of files
            var globFiles = FileHelper.MatchGlobFiles(basePath, objectSource);
            sourceFiles.UnionWith(globFiles);
        }

        // Load all the aggregate DTOs from the files
        var aggregateDtos = new List<T>();
        foreach (var sourceFile in sourceFiles)
        {
            // Deserialize the context JSON file into a DTO
            var objectDto = JsonHelper.DeserializeFromFile<T>(sourceFile);

            // If deserialization was successful, add the DTO to the list
            if (objectDto.IsSuccess)
            {
                aggregateDtos.Add(objectDto.Value);
            }
            else
            {
                // If deserialization failed, log a warning
                _logger.LogWarning("Failed to deserialize object file {AggregateFile} for type {Type}: {ErrorMessage}", sourceFile, typeof(T).Name, objectDto.GetErrorMessage());
            }
        }

        // Return all the 
        return aggregateDtos;
    }

    /// <summary>
    /// Deserializes the given source string into an NDSProject object.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Result<NDSProject> Deserialize(string source)
    {
        // Load the JSON file into a DTO
        var dto = JsonHelper.DeserializeFromFile<JsonProjectDTO>(source);

        // Check for deserialization errors
        if (!dto.IsSuccess) return Result<NDSProject>.Error(dto.GetErrorMessage());

        // Get the project version and return an error result if it is invalid
        var projectVersion = NDSVersion.Create(dto.Value.Version);
        if(!projectVersion.IsSuccess) return Result<NDSProject>.Error(projectVersion.GetErrorMessage());

        // Get the model version and return an error result if it is invalid
        var modelVersion = NDSVersion.Create(dto.Value.ModelVersion);
        if(!modelVersion.IsSuccess) return Result<NDSProject>.Error(modelVersion.GetErrorMessage());

        // Create the project details and return an error result if it is invalid
        var projectDetails = NDSProjectDetails.Create(dto.Value.Name, dto.Value.Description, projectVersion.Value, modelVersion.Value);
        if(!projectDetails.IsSuccess) return Result<NDSProject>.Error(projectDetails.GetErrorMessage());

        // Create the project
        var projectResult = NDSProject.Create(Guid.NewGuid().ToString(), projectDetails.Value);
        if(!projectResult.IsSuccess) return Result<NDSProject>.Error(projectResult.GetErrorMessage());

        // Get the created project instance
        var project = projectResult.Value;

        // Iterate over all the contexts in the dto and convert it to the domain entity
        foreach (var contextSource in dto.Value.Contexts)
        {
            // Deserialize the context JSON file into a DTO
            var ndsContextDto = JsonHelper.DeserializeFromFile<JsonContextDTO>(contextSource);
            if (!ndsContextDto.IsSuccess) continue;

            // Get the directory of the context source file to resolve relative paths
            var contextDir = Path.GetDirectoryName(contextSource) ?? throw new InvalidOperationException("Context source directory could not be determined.");

            // Load all the NDS objects from their respective sources
            var aggregateDTOs = LoadNDSObjectsFromSources<JsonAggregateDTO>(contextDir, ndsContextDto.Value.Sources.Aggregates);
            var entityDTOs = LoadNDSObjectsFromSources<JsonEntityDTO>(contextDir, ndsContextDto.Value.Sources.Entities);
            var enumDTOs = LoadNDSObjectsFromSources<JsonEnumDTO>(contextDir, ndsContextDto.Value.Sources.Enums);
            var eventDTOs = LoadNDSObjectsFromSources<JsonEventDTO>(contextDir, ndsContextDto.Value.Sources.Events);
            var operationDTOs = LoadNDSObjectsFromSources<JsonOperationDTO>(contextDir, ndsContextDto.Value.Sources.Operations);
            var valueObjectDTOs = LoadNDSObjectsFromSources<JsonValueObjectDTO>(contextDir, ndsContextDto.Value.Sources.ValueObjects);

            // Convert the DTOs to domain entities
            var aggregateEntities = aggregateDTOs.Select(a => a.ToEntity());
            var entityEntities = entityDTOs.Select(e => e.ToEntity());
            var enumEntities = enumDTOs.Select(e => e.ToEntity());
            var eventEntities = eventDTOs.Select(e => e.ToEntity());
            var operationEntities = operationDTOs.Select(o => o.ToEntity());
            var valueObjectEntities = valueObjectDTOs.Select(v => v.ToEntity());

            // Create the context object
            var version = NDSVersion.Create(ndsContextDto.Value.Version);
            var details = NDSContextDetails.Create(ndsContextDto.Value.Name, ndsContextDto.Value.Description, version.Value);
            var context = NDSContext.Create(ndsContextDto.Value.Id, details.Value);

            // Collect all the results from adding symbols
            var addSymbolResults = new List<Result>();

            // Add all the loaded symbols to the context and capture the results
            addSymbolResults.AddRange(context.AddSymbols(aggregateEntities.Cast<INDSContextObject>()));
            addSymbolResults.AddRange(context.AddSymbols(entityEntities.Cast<INDSContextObject>()));
            addSymbolResults.AddRange(context.AddSymbols(enumEntities.Cast<INDSContextObject>()));
            addSymbolResults.AddRange(context.AddSymbols(eventEntities.Cast<INDSContextObject>()));
            addSymbolResults.AddRange(context.AddSymbols(operationEntities.Cast<INDSContextObject>()));
            addSymbolResults.AddRange(context.AddSymbols(valueObjectEntities.Cast<INDSContextObject>()));

            // Check for any errors when adding symbols
            var errors = addSymbolResults.Where(r => !r.IsSuccess).Select(r => r.GetErrorMessage()).ToList();

            // If there were errors, log them
            if (errors.Count > 0)
            {
                // If there were errors, log them and skip adding this context
                _logger.LogWarning("Failed to add symbols to context {ContextId}: {Errors}", context.Id, string.Join("; ", errors));
            }

            // Add the context to the project
            var appendResult = project.AppendContext(context);
            if(!appendResult.IsSuccess) _logger.LogWarning("Failed to append context {ContextId} to project: {ErrorMessage}", context.Id, appendResult.GetErrorMessage());
        }

        // Return the project
        return project;
    }
}
