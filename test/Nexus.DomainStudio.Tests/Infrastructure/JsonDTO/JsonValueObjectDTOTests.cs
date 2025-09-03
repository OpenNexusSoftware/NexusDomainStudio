using System;
using System.IO;
using System.Linq;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonValueObjectDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-valueobject", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidValueObject_PopulatesAllFields()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "DepotId.vo.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "VO.DepotId",
              "name": "DepotId",
              "description": "Value object for depot identifier",
              "properties": [
                { "name": "Value", "type": "string", "description": "Underlying string value" }
              ]
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonValueObjectDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("VO.DepotId", dto.Id);
            Assert.Equal("DepotId", dto.Name);
            Assert.Equal("Value object for depot identifier", dto.Description);

            Assert.NotNull(dto.Properties);
            Assert.Single(dto.Properties);

            var prop = dto.Properties.First();
            Assert.Equal("Value", prop.Name); // mapped from "id" in JsonPropertyDTO
            Assert.Equal("string", prop.Type);
            Assert.Equal("Underlying string value", prop.Description);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_PartialJson_UsesDefaults()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.vo.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "VO.Minimal",
              "name": "Minimal"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonValueObjectDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("VO.Minimal", dto.Id);
            Assert.Equal("Minimal", dto.Name);
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
        var path = Path.Combine(dir, "invalid.vo.json");
        File.WriteAllText(path, "{ not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonValueObjectDTO>(path);

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
        var missing = Path.Combine(dir, "nope.vo.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonValueObjectDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }
}

