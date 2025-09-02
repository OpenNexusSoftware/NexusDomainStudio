using System;
using System.IO;
using System.Linq;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonEventDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-event", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidEvent_PopulatesAllFields()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "DepotCreated.event.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Evt.DepotCreated",
              "name": "DepotCreated",
              "description": "Raised when a depot is created",
              "properties": [
                { "name": "DepotId", "type": "string", "description": "Unique depot identifier" },
                { "name": "CreatedAt", "type": "datetime", "description": "Creation timestamp" }
              ]
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonEventDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Evt.DepotCreated", dto.Id);
            Assert.Equal("DepotCreated", dto.Name);
            Assert.Equal("Raised when a depot is created", dto.Description);

            Assert.NotNull(dto.Properties);
            Assert.Equal(2, dto.Properties.Count());

            var depotId = dto.Properties.FirstOrDefault(p => p.Name == "DepotId");
            Assert.NotNull(depotId);
            Assert.Equal("string", depotId!.Type);
            Assert.Equal("Unique depot identifier", depotId.Description);

            var createdAt = dto.Properties.FirstOrDefault(p => p.Name == "CreatedAt");
            Assert.NotNull(createdAt);
            Assert.Equal("datetime", createdAt!.Type);
            Assert.Equal("Creation timestamp", createdAt.Description);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_PartialJson_UsesDefaults()
    {
        // Arrange: omit description and properties
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.event.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Evt.Minimal",
              "name": "MinimalEvent"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonEventDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Evt.Minimal", dto.Id);
            Assert.Equal("MinimalEvent", dto.Name);

            // Defaults
            Assert.Equal(string.Empty, dto.Description);
            Assert.NotNull(dto.Properties);
            Assert.Empty(dto.Properties);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_InvalidJson_ReturnsError()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "invalid.event.json");
        File.WriteAllText(path, "{ not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEventDTO>(path);

            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to deserialize file", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_FileMissing_ReturnsError()
    {
        var dir = CreateTempDir();
        var missing = Path.Combine(dir, "nope.event.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEventDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }
}

