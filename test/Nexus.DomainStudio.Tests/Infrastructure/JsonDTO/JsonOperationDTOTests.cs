using System;
using System.IO;
using System.Linq;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonOperationDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-operation", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidOperation_WithNestedEmitsAndMappings_PopulatesAllFields()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "CreateDepot.op.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Op.CreateDepot",
              "name": "CreateDepot",
              "description": "Operation to create a depot",
              "emits": [
                {
                  "event": "Evt.DepotCreated",
                  "description": "Emits when depot is created",
                  "maps": [
                    { "property": "DepotId", "value": "$.Input.DepotId" },
                    { "property": "CreatedAt", "value": "$.Now" }
                  ]
                },
                {
                  "event": "Evt.AuditLog",
                  "description": "Audit entry",
                  "maps": [
                    { "property": "Message", "value": "Depot created" }
                  ]
                }
              ]
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonOperationDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Op.CreateDepot", dto.Id);
            Assert.Equal("CreateDepot", dto.Name);
            Assert.Equal("Operation to create a depot", dto.Description);

            Assert.NotNull(dto.Emits);
            Assert.Equal(2, dto.Emits.Count());

            var firstEmit = dto.Emits.First();
            Assert.Equal("Evt.DepotCreated", firstEmit.EventId);
            Assert.Equal("Emits when depot is created", firstEmit.Description);
            Assert.NotNull(firstEmit.Mappings);
            Assert.Equal(2, firstEmit.Mappings.Count());
            Assert.Contains(firstEmit.Mappings, m => m.Property == "DepotId" && m.Value == "$.Input.DepotId");
            Assert.Contains(firstEmit.Mappings, m => m.Property == "CreatedAt" && m.Value == "$.Now");

            var secondEmit = dto.Emits.Skip(1).First();
            Assert.Equal("Evt.AuditLog", secondEmit.EventId);
            Assert.Equal("Audit entry", secondEmit.Description);
            Assert.Single(secondEmit.Mappings);
            Assert.Equal("Message", secondEmit.Mappings.First().Property);
            Assert.Equal("Depot created", secondEmit.Mappings.First().Value);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_Operation_PartialJson_UsesDefaults()
    {
        // Arrange: omit description and emits
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.op.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Op.Minimal",
              "name": "Minimal"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonOperationDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Op.Minimal", dto.Id);
            Assert.Equal("Minimal", dto.Name);
            Assert.Equal(string.Empty, dto.Description);
            Assert.NotNull(dto.Emits);
            Assert.Empty(dto.Emits);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_Emit_PartialJson_UsesDefaults()
    {
        // Arrange: only event id provided, maps/description omitted
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "emit.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "event": "Evt.SomethingHappened"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonOperationEmitDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Evt.SomethingHappened", dto.EventId);
            Assert.Equal(string.Empty, dto.Description);
            Assert.NotNull(dto.Mappings);
            Assert.Empty(dto.Mappings);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_PropertyMapping_PartialJson_UsesDefaults()
    {
        // Arrange: only property provided
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "map.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "property": "DepotId"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonPropertyMappingDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("DepotId", dto.Property);
            Assert.Equal(string.Empty, dto.Value); // default
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_InvalidJson_ReturnsError()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "invalid.op.json");
        File.WriteAllText(path, "{ not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonOperationDTO>(path);

            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to deserialize file", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_FileMissing_ReturnsError()
    {
        var dir = CreateTempDir();
        var missing = Path.Combine(dir, "nope.op.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonOperationDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}
