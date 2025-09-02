using System;
using System.IO;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonArgumentDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-argument", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidArgument_PopulatesAllFields()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "argument.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "name": "limit",
              "type": "int",
              "required": false,
              "description": "Maximum number of items"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonArgumentDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("limit", dto.Name);
            Assert.Equal("int", dto.Type);
            Assert.False(dto.Required); // explicitly set in JSON
            Assert.Equal("Maximum number of items", dto.Description);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_PartialJson_UsesDefaults()
    {
        // Arrange: omit type/description/required to test defaults
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.argument.json");

        File.WriteAllText(path, /* language=json */ """
            { "name": "search" }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonArgumentDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("search", dto.Name);
            Assert.Equal(string.Empty, dto.Type);
            Assert.Equal(string.Empty, dto.Description);
            Assert.True(dto.Required); // default is true
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
        var missing = Path.Combine(dir, "does-not-exist.json");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonArgumentDTO>(missing);

            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
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
        var path = Path.Combine(dir, "invalid.argument.json");
        File.WriteAllText(path, "{ this is not valid json ");

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonArgumentDTO>(path);

            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to deserialize file", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}

