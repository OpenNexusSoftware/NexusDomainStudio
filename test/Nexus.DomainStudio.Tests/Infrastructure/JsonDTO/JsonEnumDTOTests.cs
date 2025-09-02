using System;
using System.IO;
using System.Linq;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonEnumDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-enum", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidEnumFile_PopulatesAllFields()
    {
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "DepotType.enum.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Enum.DepotType",
              "name": "DepotType",
              "description": "Types of depots",
              "values": ["Main", "Secondary", "Virtual"]
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEnumDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Enum.DepotType", dto.Id);
            Assert.Equal("DepotType", dto.Name);
            Assert.Equal("Types of depots", dto.Description);

            Assert.NotNull(dto.Values);
            Assert.Equal(3, dto.Values.Count());
            Assert.Contains("Main", dto.Values);
            Assert.Contains("Secondary", dto.Values);
            Assert.Contains("Virtual", dto.Values);
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
        var path = Path.Combine(dir, "partial.enum.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Enum.Minimal",
              "name": "Minimal"
            }
            """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEnumDTO>(path);

            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Enum.Minimal", dto.Id);
            Assert.Equal("Minimal", dto.Name);
            Assert.Equal(string.Empty, dto.Description);

            Assert.NotNull(dto.Values);
            Assert.Empty(dto.Values); // default []
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
        var path = Path.Combine(dir, "invalid.enum.json");
        File.WriteAllText(path, "{ not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEnumDTO>(path);

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
        var missing = Path.Combine(dir, "nope.enum.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonEnumDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}

