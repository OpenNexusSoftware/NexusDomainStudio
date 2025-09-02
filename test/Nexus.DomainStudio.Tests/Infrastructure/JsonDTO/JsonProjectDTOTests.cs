using Nexus.DomainStudio.Infrastructure.Persistence.DataObjects;
using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.JsonDTO;

public class JsonProjectDTOTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "json-project-dto", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    [Fact]
    public void Deserialize_ValidFile_ReturnsDTOWithValues()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "project.json");

        File.WriteAllText(path, /* language=json */ """
        {
            "name": "Test_Project",
            "version": "1.0.0",
            "modelVersion": "1.0.0",
            "description": "This is a test project.",
            "contexts": [
                "models/depots/context.json",
                "models/logistics/context.json",
                "models/products/context.json",
                "models/users/context.json"
            ]
        }
        """);

        try
        {
            var result = JsonHelper.DeserializeFromFile<JsonProjectDTO>(path);

            Assert.True(result.IsSuccess);
            var dto = result.Value;

            Assert.Equal("Test_Project", dto.Name);
            Assert.Equal("1.0.0", dto.Version);
            Assert.Equal("1.0.0", dto.ModelVersion);
            Assert.Equal("This is a test project.", dto.Description);
            Assert.Equal(4, dto.Contexts.Count());
            Assert.Contains("models/depots/context.json", dto.Contexts);
            Assert.Contains("models/logistics/context.json", dto.Contexts);
            Assert.Contains("models/products/context.json", dto.Contexts);
            Assert.Contains("models/users/context.json", dto.Contexts);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { /* ignore */ }
        }
    }

    [Fact]
    public void Deserialize_FileMissing_ReturnsError()
    {
        // Arrange
        var dir = CreateTempDir();
        var missingPath = Path.Combine(dir, "does-not-exist.json");

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonProjectDTO>(missingPath);

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
        var path = Path.Combine(dir, "invalid.json");
        File.WriteAllText(path, "{ this is not valid json ");

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonProjectDTO>(path);

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
    public void Deserialize_MissingOptionalFields_UsesDefaults()
    {
        // Arrange
        var dir = CreateTempDir();
        var path = Path.Combine(dir, "partial.json");

        // Only include 'name' to verify defaults on others.
        File.WriteAllText(path, /* language=json */ """
            {
              "name": "Minimal"
            }
            """);

        try
        {
            // Act
            var result = JsonHelper.DeserializeFromFile<JsonProjectDTO>(path);

            // Assert
            Assert.True(result.IsSuccess, result.GetErrorMessage());
            var dto = result.Value;

            Assert.Equal("Minimal", dto.Name);
            Assert.NotNull(dto.Version);
            Assert.NotNull(dto.ModelVersion);
            Assert.NotNull(dto.Description);
            Assert.NotNull(dto.Contexts);
            Assert.Empty(dto.Contexts);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }
}
