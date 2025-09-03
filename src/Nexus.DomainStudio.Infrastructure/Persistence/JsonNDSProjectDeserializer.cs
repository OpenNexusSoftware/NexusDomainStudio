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
/// Helper class to hold the results of loading NDS objects, separating successful loads from failed ones.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class LoadNDSObjectsResult<T> where T : class
{
    public List<T> Successful { get; init; } = [];
    public List<Result<T>> Failed { get; init; } = [];
}

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
    private LoadNDSObjectsResult<T> LoadNDSObjectsFromSources<T>(string basePath, IEnumerable<string> sources) where T : class
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
        var dtoResults = new List<T>();
        var dtoFails = new List<Result<T>>();
        foreach (var sourceFile in sourceFiles)
        {
            // Deserialize the context JSON file into a DTO
            var objectDtoResult = JsonHelper.DeserializeFromFile<T>(sourceFile);

            if(objectDtoResult.IsSuccess)
            {
                // If deserialization was successful, add the DTO to the list
                dtoResults.Add(objectDtoResult.Value);
            }
            else
            {
                // If deserialization failed, log a warning and add the error to the list
                _logger.LogWarning("Failed to deserialize object file {ObjectFile}: {ErrorMessage}", sourceFile, objectDtoResult.GetErrorMessage());
                dtoFails.Add(Result<T>.Error($"Failed to deserialize object file {sourceFile}: {objectDtoResult.GetErrorMessage()}"));
            }
        }

        // Return all the failed and the successful results
        return new LoadNDSObjectsResult<T>
        {
            Successful = dtoResults,
            Failed = dtoFails
        };
    }

    /// <summary>
    /// Deserializes the given source string into an NDSProject object.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Result<JsonNDSProjectDeserializedResult> Deserialize(string source)
    {
        // Load the JSON file into a DTO
        var dto = JsonHelper.DeserializeFromFile<JsonProjectDTO>(source);

        // Check for deserialization errors
        if (!dto.IsSuccess) return Result<JsonNDSProjectDeserializedResult>.Error(dto.GetErrorMessage());

        // Get the project version and return an error result if it is invalid
        var projectVersion = NDSVersion.Create(dto.Value.Version);
        if(!projectVersion.IsSuccess) return Result<JsonNDSProjectDeserializedResult>.Error(projectVersion.GetErrorMessage());

        // Get the model version and return an error result if it is invalid
        var modelVersion = NDSVersion.Create(dto.Value.ModelVersion);
        if(!modelVersion.IsSuccess) return Result<JsonNDSProjectDeserializedResult>.Error(modelVersion.GetErrorMessage());

        // Create the project details and return an error result if it is invalid
        var projectDetails = NDSProjectDetails.Create(dto.Value.Name, dto.Value.Description, projectVersion.Value, modelVersion.Value);
        if(!projectDetails.IsSuccess) return Result<JsonNDSProjectDeserializedResult>.Error(projectDetails.GetErrorMessage());

        // Create the project
        var projectResult = NDSProject.Create(Guid.NewGuid().ToString(), projectDetails.Value);
        if(!projectResult.IsSuccess) return Result<JsonNDSProjectDeserializedResult>.Error(projectResult.GetErrorMessage());

        // Get the created project instance and create the deserialized result
        var deserializeResult = new JsonNDSProjectDeserializedResult(projectResult.Value);

        // Iterate over all the contexts in the dto and convert it to the domain entity
        foreach (var contextSource in dto.Value.Contexts)
        {
            // Get the full path to the context source file
            var contextPath = Path.Combine(Path.GetDirectoryName(source) ?? string.Empty, contextSource);

            // Deserialize the context JSON file into a DTO
            var ndsContextDto = JsonHelper.DeserializeFromFile<JsonContextDTO>(contextPath);
            if (!ndsContextDto.IsSuccess)
            {
                // If deserialization failed, log a warning and skip this context
                _logger.LogWarning("Failed to deserialize context file {ContextFile}: {ErrorMessage}", contextPath, ndsContextDto.GetErrorMessage());
                deserializeResult.DeserialiseErrors.Add(Result.Error($"Failed to deserialize context file {contextPath}: {ndsContextDto.GetErrorMessage()}"));
                continue;
            }

            // Get the directory of the context source file to resolve relative paths
            var contextDir = Path.GetDirectoryName(contextPath) ?? throw new InvalidOperationException("Context source directory could not be determined.");

            // Load all the NDS objects from their respective sources
            var aggregateDTOResult = LoadNDSObjectsFromSources<JsonAggregateDTO>(contextDir, ndsContextDto.Value.Sources.Aggregates);
            var entityDTOResult = LoadNDSObjectsFromSources<JsonEntityDTO>(contextDir, ndsContextDto.Value.Sources.Entities);
            var enumDTOResult = LoadNDSObjectsFromSources<JsonEnumDTO>(contextDir, ndsContextDto.Value.Sources.Enums);
            var eventDTOResult = LoadNDSObjectsFromSources<JsonEventDTO>(contextDir, ndsContextDto.Value.Sources.Events);
            var operationDTOResult = LoadNDSObjectsFromSources<JsonOperationDTO>(contextDir, ndsContextDto.Value.Sources.Operations);
            var valueObjectDTOResult = LoadNDSObjectsFromSources<JsonValueObjectDTO>(contextDir, ndsContextDto.Value.Sources.ValueObjects);

            // Collect any deserialization errors
            deserializeResult.DeserialiseErrors.AddRange(aggregateDTOResult.Failed.Select(f => Result.Error(f.GetErrorMessage())));
            deserializeResult.DeserialiseErrors.AddRange(entityDTOResult.Failed.Select(f => Result.Error(f.GetErrorMessage())));
            deserializeResult.DeserialiseErrors.AddRange(enumDTOResult.Failed.Select(f => Result.Error(f.GetErrorMessage())));
            deserializeResult.DeserialiseErrors.AddRange(eventDTOResult.Failed.Select(f => Result.Error(f.GetErrorMessage())));
            deserializeResult.DeserialiseErrors.AddRange(operationDTOResult.Failed.Select(f => Result.Error(f.GetErrorMessage())));
            deserializeResult.DeserialiseErrors.AddRange(valueObjectDTOResult.Failed.Select(f => Result.Error(f.GetErrorMessage())));

            // Convert the DTOs to domain entities
            var aggregateEntities = aggregateDTOResult.Successful.Select(a => a.ToEntity());
            var entityEntities = entityDTOResult.Successful.Select(e => e.ToEntity());
            var enumEntities = enumDTOResult.Successful.Select(e => e.ToEntity());
            var eventEntities = eventDTOResult.Successful.Select(e => e.ToEntity());
            var operationEntities = operationDTOResult.Successful.Select(o => o.ToEntity());
            var valueObjectEntities = valueObjectDTOResult.Successful.Select(v => v.ToEntity());

            // Combine all the conversion results to Result<INDSContextObject> items
            var conversionObjects = aggregateEntities.Select(r => r.IsSuccess ? Result<INDSContextObject>.Success(r.Value) : Result<INDSContextObject>.Error(r.GetErrorMessage()));
            var successfulConversions = new List<Result<INDSContextObject>>();

            // Partition the results into successful and failed conversions
            conversionObjects.PartitionInto(successfulConversions, deserializeResult.ConversionErrors, r => r.IsSuccess);

            // Create the context object
            var version = NDSVersion.Create(ndsContextDto.Value.Version);
            var details = NDSContextDetails.Create(ndsContextDto.Value.Name, ndsContextDto.Value.Description, version.Value);
            var context = NDSContext.Create(ndsContextDto.Value.Id, details.Value);

            // Collect all the results from adding symbols
            var addSymbolResults = context.AddSymbols(successfulConversions.Select(r => r.Value));

            // Add all the loaded symbols to the context and capture the results
            // addSymbolResults.AddRange(context.AddSymbols(aggregateEntities.Select(a => a.Value as INDSContextObject)));
            // addSymbolResults.AddRange(context.AddSymbols(entityEntities.Select(a => a.Value as INDSContextObject)));
            // addSymbolResults.AddRange(context.AddSymbols(enumEntities.Select(a => a.Value as INDSContextObject)));
            // addSymbolResults.AddRange(context.AddSymbols(eventEntities.Select(a => a.Value as INDSContextObject)));
            // addSymbolResults.AddRange(context.AddSymbols(operationEntities.Select(a => a.Value as INDSContextObject)));
            // addSymbolResults.AddRange(context.AddSymbols(valueObjectEntities.Select(a => a.Value as INDSContextObject)));

            // Check for any errors when adding symbols
            var errors = addSymbolResults.Where(r => !r.IsSuccess).Select(r => r.GetErrorMessage()).ToList();

            // If there were errors, log them
            if (errors.Count > 0)
            {
                // If there were errors, log them and skip adding this context
                _logger.LogWarning("Failed to add symbols to context {ContextId}: {Errors}", context.Id, string.Join("; ", errors));
            }

            // Add the context to the project
            var appendResult = deserializeResult.Project.AppendContext(context);
            if(!appendResult.IsSuccess) _logger.LogWarning("Failed to append context {ContextId} to project: {ErrorMessage}", context.Id, appendResult.GetErrorMessage());
        }

        // Return the deserializeResult
        return deserializeResult;
    }
}
