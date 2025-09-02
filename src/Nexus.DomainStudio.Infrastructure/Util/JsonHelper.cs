using System.Text.Json;
using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Infrastructure.Util;

/// <summary>
/// Helper class for JSON serialization and deserialization.
/// </summary>
public static class JsonHelper
{
    public static Result<T> DeserializeFromFile<T>(string filePath) where T : class
    {
        // Check if the given file exists, if it does not return an error result
        var fileExists = File.Exists(filePath);
        if(!fileExists) return Result<T>.Error($"File not found: {filePath}");

        // Read the file contents so we can deserialize it
        var fileContents = File.ReadAllText(filePath);
        T? dto = default(T);

        try
        {
            // Try and deserialize the file contents into the given type
            dto = JsonSerializer.Deserialize<T>(fileContents);
        }
        catch(Exception ex)
        {
            // If deserialization fails, return an error result with the exception message
            return Result<T>.Error($"Failed to deserialize file: {ex.Message}");
        }

        // The deserialized object here should never be null, so guard against it
        Guard.AgainstNull(dto, typeof(T).Name);

        // Return a successful result containing the deserialized object
        return dto!;
    }
}