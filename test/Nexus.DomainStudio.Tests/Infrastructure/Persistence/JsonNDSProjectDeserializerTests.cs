using Microsoft.Extensions.Logging;
using Nexus.DomainStudio.Infrastructure.Persistence;

namespace Nexus.DomainStudio.Tests.Infrastructure.Persistence;

public class JsonNDSProjectDeserializerTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "project-deser", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }


    private static ILogger<JsonNDSProjectDeserializer> CreateLogger()
    {
        using var factory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Debug));
        return LoggerFactory
            .Create(b => b.SetMinimumLevel(LogLevel.Debug))
            .CreateLogger<JsonNDSProjectDeserializer>();
    }

    [Fact]
    public void Deserialize_ProjectWithOneContextAndNoSources_Succeeds_AndAddsContext()
    {
        var root = CreateTempDir();
        try
        {
            // Arrange: file layout
            // <root>\project.nds.json
            // <root>\contexts\depots.context.json  (with empty sources)
            var contextsDir = Path.Combine(root, "contexts");
            Directory.CreateDirectory(contextsDir);

            var projectPath = Path.Combine(root, "project.nds.json");
            var depotsContextPath = Path.Combine(contextsDir, "depots.context.json");

            File.WriteAllText(projectPath, /* language=json */ """
                {
                  "name": "Demo Project",
                  "version": "1.0.0",
                  "modelVersion": "1.0.0",
                  "description": "Test project",
                  "contexts": [
                    "contexts/depots.context.json"
                  ]
                }
                """);

            File.WriteAllText(depotsContextPath, /* language=json */ """
                {
                  "id": "depots",
                  "name": "Depots",
                  "description": "Domain for depots",
                  "version": "1.0.0",
                  "sources": {
                    "aggregates": [],
                    "entities": [],
                    "enums": [],
                    "events": [],
                    "operations": [],
                    "valueObjects": []
                  }
                }
                """);

            var logger = CreateLogger();
            var sut = new JsonNDSProjectDeserializer(logger);

            // Act
            var result = sut.Deserialize(projectPath);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var project = result.Value;

            // Should have exactly one context: "depots"
            Assert.NotNull(project.Project.Contexts);
            Assert.True(project.Project.Contexts.Count == 1, $"Expected 1 context, got {project.Project.Contexts.Count}");
            Assert.Contains("depots", project.Project.Contexts.Keys);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { /* best effort */ }
        }
    }

    [Fact]
    public void Deserialize_InvalidProjectJson_ReturnsError()
    {
        var root = CreateTempDir();
        try
        {
            var projectPath = Path.Combine(root, "project.nds.json");
            File.WriteAllText(projectPath, "{ invalid json ");

            var logger = CreateLogger();
            var sut = new JsonNDSProjectDeserializer(logger);

            var result = sut.Deserialize(projectPath);

            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to deserialize", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_MissingContextFile_ContinuesAndReturnsProjectWithZeroContexts()
    {
        var root = CreateTempDir();
        try
        {
            var projectPath = Path.Combine(root, "project.nds.json");
            // Point to a context file that does not exist
            File.WriteAllText(projectPath, /* language=json */ """
                {
                  "name": "Demo Project",
                  "version": "1.0.0",
                  "modelVersion": "1.0.0",
                  "description": "Test project",
                  "contexts": [
                    "contexts/missing.context.json"
                  ]
                }
                """);

            var logger = CreateLogger();
            var sut = new JsonNDSProjectDeserializer(logger);

            var result = sut.Deserialize(projectPath);

            // Current implementation: it 'continue's if a context fails to deserialize
            Assert.True(result.IsSuccess, result.GetErrorMessage());

            var project = result.Value;
            Assert.NotNull(project.Project.Contexts);
            Assert.Empty(project.Project.Contexts); // no contexts appended
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_ProjectWithMultipleContexts_AllLoaded_WhenValid()
    {
        var root = CreateTempDir();
        try
        {
            var contextsDir = Path.Combine(root, "contexts");
            Directory.CreateDirectory(contextsDir);

            var projectPath = Path.Combine(root, "project.nds.json");
            var depotsContextPath = Path.Combine(contextsDir, "depots.context.json");
            var inventoryContextPath = Path.Combine(contextsDir, "inventory.context.json");

            File.WriteAllText(projectPath, /* language=json */ """
                {
                  "name": "MultiCtx",
                  "version": "1.0.0",
                  "modelVersion": "1.0.0",
                  "description": "Two contexts",
                  "contexts": [
                    "contexts/depots.context.json",
                    "contexts/inventory.context.json"
                  ]
                }
                """);

            var emptySources = """
                {
                  "aggregates": [],
                  "entities": [],
                  "enums": [],
                  "events": [],
                  "operations": [],
                  "valueObjects": []
                }
                """;

            File.WriteAllText(depotsContextPath, $$"""
                {
                    "id": "depots",
                    "name": "Depots",
                    "description": "Depots domain",
                    "version": "1.0.0",
                    "sources": {{emptySources}}
                }
                """);

            File.WriteAllText(inventoryContextPath, $$"""
                {

                    "id": "inventory",
                    "name": "Inventory",
                    "description": "Inventory domain",
                    "version": "1.0.0",
                    "sources": {{emptySources}}
                }
                """);

            var logger = CreateLogger();
            var sut = new JsonNDSProjectDeserializer(logger);

            var result = sut.Deserialize(projectPath);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var project = result.Value;

            Assert.Equal(2, project.Project.Contexts.Count);
            Assert.Contains("depots", project.Project.Contexts.Keys);
            Assert.Contains("inventory", project.Project.Contexts.Keys);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_ContextWithAllSourceTypes_Succeeds()
    {
        var root = CreateTempDir();
        try
        {
            // directories for sources
            var contextsDir = Path.Combine(root, "contexts");
            var aggregatesDir = Path.Combine(contextsDir, "aggregates");
            var entitiesDir = Path.Combine(contextsDir, "entities");
            var enumsDir = Path.Combine(contextsDir, "enums");
            var eventsDir = Path.Combine(contextsDir, "events");
            var operationsDir = Path.Combine(contextsDir, "operations");
            var voDir = Path.Combine(contextsDir, "value_objects");

            Directory.CreateDirectory(contextsDir);
            Directory.CreateDirectory(aggregatesDir);
            Directory.CreateDirectory(entitiesDir);
            Directory.CreateDirectory(enumsDir);
            Directory.CreateDirectory(eventsDir);
            Directory.CreateDirectory(operationsDir);
            Directory.CreateDirectory(voDir);

            // project.json
            var projectPath = Path.Combine(root, "project.nds.json");
            var contextPath = Path.Combine(contextsDir, "depots.context.json");

            File.WriteAllText(projectPath, """
                {
                  "name": "Demo Full Project",
                  "version": "1.0.0",
                  "modelVersion": "1.0.0",
                  "description": "Test project with full sources",
                  "contexts": [
                    "contexts/depots.context.json"
                  ]
                }
                """);

            // context.json
            File.WriteAllText(contextPath, """
                {
                  "id": "depots",
                  "name": "Depots",
                  "description": "Depots domain",
                  "version": "1.0.0",
                  "sources": {
                    "aggregates": [ "aggregates/Depot.aggregate.json" ],
                    "entities":   [ "entities/Depot.entity.json" ],
                    "enums":      [ "enums/DepotType.enum.json" ],
                    "events":     [ "events/DepotCreated.event.json" ],
                    "operations": [ "operations/CreateDepot.op.json" ],
                    "valueObjects": [ "value_objects/DepotId.vo.json" ]
                  }
                }
                """);

            // add one file for each source
            File.WriteAllText(Path.Combine(aggregatesDir, "Depot.aggregate.json"), """
                { "id": "agg1", "name": "DepotAgg", "description": "Aggregate root", "root": "Depot" }
                """);

            File.WriteAllText(Path.Combine(entitiesDir, "Depot.entity.json"), """
                { "id": "ent1", "name": "Depot", "description": "Entity for depot", "properties": [], "operations": [] }
                """);

            File.WriteAllText(Path.Combine(enumsDir, "DepotType.enum.json"), """
                { "id": "enum1", "name": "DepotType", "description": "Enum", "values": [ "Small", "Large" ] }
                """);

            File.WriteAllText(Path.Combine(eventsDir, "DepotCreated.event.json"), """
                { "id": "evt1", "name": "DepotCreated", "description": "Event", "properties": [] }
                """);

            File.WriteAllText(Path.Combine(operationsDir, "CreateDepot.op.json"), """
                { "id": "op1", "name": "CreateDepot", "description": "Operation", "emits": [] }
                """);

            File.WriteAllText(Path.Combine(voDir, "DepotId.vo.json"), """
                { "id": "vo1", "name": "DepotId", "description": "Value Object", "properties": [] }
                """);

            var logger = CreateLogger();
            var sut = new JsonNDSProjectDeserializer(logger);

            // Act
            var result = sut.Deserialize(projectPath);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            Assert.Empty(result.Value.ConversionErrors); // no conversion errors
            Assert.Empty(result.Value.DeserialiseErrors); // no deserialization errors
            
            var project = result.Value;

            // Should have one context
            Assert.Single(project.Project.Contexts);
            Assert.Contains("depots", project.Project.Contexts.Keys);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }
}

