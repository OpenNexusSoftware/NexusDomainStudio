using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonContextDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-context-dto", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidContextFile_PopulatesAllFields()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "depots.context.json");

        // A full, valid context JSON
        File.WriteAllText(path, /* language=json */ """
            {
              "id": "depots",
              "name": "Depots",
              "description": "Domain for managing depots within the logistics system.",
              "version": "1.0.0",
              "sources": {
                "aggregates": ["aggregates/*.aggregate.json"],
                "entities": ["entities/*.entity.json"],
                "enums": ["enums/*.enum.json"],
                "events": ["events/*.event.json"],
                "operations": ["operations/*.op.json"],
                "valueObjects": ["value_objects/*.vo.json"]
              }
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonContextDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            // These asserts reflect the intended populated state
            Assert.Equal("depots", dto.Id);
            Assert.Equal("Depots", dto.Name);
            Assert.Equal("Domain for managing depots within the logistics system.", dto.Description);
            Assert.Equal("1.0.0", dto.Version);

            Assert.NotNull(dto.Sources);

            Assert.NotNull(dto.Sources.Aggregates);
            Assert.Single(dto.Sources.Aggregates);
            Assert.Equal("aggregates/*.aggregate.json", dto.Sources.Aggregates[0]);

            Assert.NotNull(dto.Sources.Entities);
            Assert.Single(dto.Sources.Entities);
            Assert.Equal("entities/*.entity.json", dto.Sources.Entities[0]);

            Assert.NotNull(dto.Sources.Enums);
            Assert.Single(dto.Sources.Enums);
            Assert.Equal("enums/*.enum.json", dto.Sources.Enums[0]);

            Assert.NotNull(dto.Sources.Events);
            Assert.Single(dto.Sources.Events);
            Assert.Equal("events/*.event.json", dto.Sources.Events[0]);

            Assert.NotNull(dto.Sources.Operations);
            Assert.Single(dto.Sources.Operations);
            Assert.Equal("operations/*.op.json", dto.Sources.Operations[0]);

            Assert.NotNull(dto.Sources.ValueObjects);
            Assert.Single(dto.Sources.ValueObjects);
            Assert.Equal("value_objects/*.vo.json", dto.Sources.ValueObjects[0]);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_ContextWithMissingSources_UsesEmptyArrays()
    {
        // Arrange: omit 'sources' entirely; DTO should still be constructed
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.context.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "depots",
              "name": "Depots",
              "description": "Desc",
              "version": "1.0.0"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonContextDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("depots", dto.Id);
            Assert.Equal("Depots", dto.Name);
            Assert.Equal("Desc", dto.Description);
            Assert.Equal("1.0.0", dto.Version);

            Assert.NotNull(dto.Sources);               // Should not be null (constructed with default)
            Assert.NotNull(dto.Sources.Aggregates);    // Empty arrays by default
            Assert.NotNull(dto.Sources.Entities);
            Assert.NotNull(dto.Sources.Enums);
            Assert.NotNull(dto.Sources.Events);
            Assert.NotNull(dto.Sources.Operations);
            Assert.NotNull(dto.Sources.ValueObjects);

            Assert.Empty(dto.Sources.Aggregates);
            Assert.Empty(dto.Sources.Entities);
            Assert.Empty(dto.Sources.Enums);
            Assert.Empty(dto.Sources.Events);
            Assert.Empty(dto.Sources.Operations);
            Assert.Empty(dto.Sources.ValueObjects);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_InvalidJson_ReturnsError()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "invalid.context.json");
        File.WriteAllText(path, "{ this is not valid json ");

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonContextDTO>(path);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to deserialize", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_FileMissing_ReturnsError()
    {
        // Arrange
        var dir = CreateTempDir();
        var missing = Path.Combine(dir, "nope.context.json");

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonContextDTO>(missing);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }
}