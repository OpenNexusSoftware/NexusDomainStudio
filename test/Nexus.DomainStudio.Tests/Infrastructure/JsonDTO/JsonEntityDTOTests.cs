using System;
using System.IO;
using System.Linq;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonEntityDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-entity", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidEntityFile_PopulatesAllFields()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "Depot.entity.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Ent.Depot",
              "name": "Depot",
              "description": "Entity representing a depot",
              "properties": [
                { "name": "DepotId", "type": "string", "description": "Unique depot identifier" },
                { "name": "Location", "type": "string", "description": "Depot location" }
              ],
              "operations": [ "Create", "Update", "Delete" ]
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEntityDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Ent.Depot", dto.Id);
            Assert.Equal("Depot", dto.Name);
            Assert.Equal("Entity representing a depot", dto.Description);

            Assert.NotNull(dto.Properties);
            Assert.Equal(2, dto.Properties.Count());
            Assert.Contains(dto.Properties, p => p.Name == "DepotId" && p.Type == "string");

            Assert.NotNull(dto.Operations);
            Assert.Equal(3, dto.Operations.Count());
            Assert.Contains("Create", dto.Operations);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_PartialJson_UsesDefaults()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.entity.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Ent.Minimal",
              "name": "Minimal"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEntityDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Ent.Minimal", dto.Id);
            Assert.Equal("Minimal", dto.Name);

            // Defaults
            Assert.Equal(string.Empty, dto.Description);
            Assert.NotNull(dto.Properties);
            Assert.Empty(dto.Properties);
            Assert.NotNull(dto.Operations);
            Assert.Empty(dto.Operations);
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
        var path = Path.Combine(dir, "invalid.entity.json");
        File.WriteAllText(path, "{ not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEntityDTO>(path);

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
        var missing = Path.Combine(dir, "nope.entity.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEntityDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}

