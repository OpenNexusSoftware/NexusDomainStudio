using Nexus.DomainStudio.Domain.Common;

public class Version : ValueObject
{
    public int Minor { get; private set; }
    public int Major { get; private set; }
    public int Patch { get; private set; }

    private Version(int minor, int major, int patch)
    {
        Minor = minor;
        Major = major;
        Patch = patch;
    }

    /// <summary>
    /// Creates a Version object from a version string in the format "Major.Minor.Patch".
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    public static Result<Version> Create(string version)
    {
        // Split the version string by '.'
        var parts = version.Split('.');

        // Check if we have exactly 3 parts: Major, Minor, Patch
        if (parts.Length != 3)
        {
            return Result<Version>.Error("Invalid version format. Expected format: Major.Minor.Patch");
        }

        // Try to parse each part to an integer
        var major = int.TryParse(parts[0], out var majorPart) ? majorPart : -1;
        var minor = int.TryParse(parts[1], out var minorPart) ? minorPart : -1;
        var patch = int.TryParse(parts[2], out var patchPart) ? patchPart : -1;

        // Validate that all parts are non-negative integers
        if (major < 0 || minor < 0 || patch < 0)
        {
            return Result<Version>.Error("Invalid version format. Expected format: Major.Minor.Patch");
        }

        // Create and return the Version object
        return new Version(major, minor, patch);
    }

    /// <summary>
    /// Creates a Version object from individual major, minor, and patch numbers.
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="patch"></param>
    /// <returns></returns>
    public static Result<Version> Create(int major, int minor, int patch)
    {
        if (major < 0 || minor < 0 || patch < 0)
        {
            return Result<Version>.Error("Version numbers must be non-negative.");
        }

        return new Version(major, minor, patch);
    }

    /// <summary>
    /// Gets the components that define equality for this value object.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Major;
        yield return Minor;
        yield return Patch;
    }
}
