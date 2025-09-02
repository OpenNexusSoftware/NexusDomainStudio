using System;
using System.IO;
using Xunit;
using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonAggregateDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-aggregate", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidAggregateFile_PopulatesAllFields()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "Depot.aggregate.json");

        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Agg.Depot",
              "name": "Depot",
              "description": "Aggregate for depot domain",
              "root": "DepotEntity"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonAggregateDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Agg.Depot", dto.Id);
            Assert.Equal("Depot", dto.Name);
            Assert.Equal("Aggregate for depot domain", dto.Description);
            Assert.Equal("DepotEntity", dto.Root);
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
        var missing = Path.Combine(dir, "does-not-exist.aggregate.json");

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonAggregateDTO>(missing);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("File not found", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
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
        var path = Path.Combine(dir, "invalid.aggregate.json");
        File.WriteAllText(path, "{ this is not valid json ");

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonAggregateDTO>(path);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to deserialize file", result.GetErrorMessage(), StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void Deserialize_PartialJson_UsesDefaultsForMissingFields()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.aggregate.json");

        // Only provide Id and Name; others should take default values from the DTO.
        File.WriteAllText(path, /* language=json */ """
            {
              "id": "Agg.Minimal",
              "name": "Minimal"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonAggregateDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Agg.Minimal", dto.Id);
            Assert.Equal("Minimal", dto.Name);

            // Defaults from DTO initializers
            Assert.NotNull(dto.Description);
            Assert.Equal(string.Empty, dto.Description);

            Assert.NotNull(dto.Root);
            Assert.Equal(string.Empty, dto.Root);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }
}
