using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Nexus.DomainStudio.Infrastructure.Util;

/// <summary>
/// Helper class for file operations.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Matches files in the specified base directory according to the given glob pattern.
    /// </summary>
    /// <param name="baseDir"></param>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    public static IEnumerable<string> MatchGlobFiles(string baseDir, string searchPattern)
    {
        // Check if the base directory exists
        if (!Directory.Exists(baseDir))
            return [];

        // If the base path is null, return an empty list
        if(baseDir == null) return [];

        // Create a new matcher and add the search pattern to include
        var matcher = new Matcher();
        matcher.AddInclude(searchPattern);

        // Search for files in the base path
        var result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(baseDir ?? string.Empty)));

        // Return the matched file paths
        return result.Files.Select(file => Path.Combine(baseDir ?? string.Empty, file.Path));
    }
}