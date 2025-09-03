using System;
using System.IO;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonPropertyDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-property", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidProperty_PopulatesAllFields()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "DepotId.property.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "DepotId",
              "type": "string",
              "description": "Unique depot identifier"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonPropertyDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("DepotId", dto.Name);
            Assert.Equal("string", dto.Type);
            Assert.Equal("Unique depot identifier", dto.Description);
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
        var path = Path.Combine(dir, "partial.property.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "name": "Location"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonPropertyDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Location", dto.Name);
            Assert.Equal(string.Empty, dto.Type);
            Assert.Equal(string.Empty, dto.Description);
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
        var path = Path.Combine(dir, "invalid.property.json");
        File.WriteAllText(path, "{ this is not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonPropertyDTO>(path);

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
        var missing = Path.Combine(dir, "does-not-exist.property.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonPropertyDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}

