using Nexus.DomainStudio.Domain.Common;

public class NDSProjectDetails : ValueObject
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Version Version { get; private set; }
    public Version ModelVersion { get; private set; }

    private NDSProjectDetails(string name, string description, Version version, Version modelVersion)
    {
        Name = name;
        Description = description;
        Version = version;
        ModelVersion = modelVersion;
    }

    public static NDSProjectDetails Create(string name, string description, Version version, Version modelVersion)
    {
        // Add any necessary validation here
        return new NDSProjectDetails(name, description, version, modelVersion);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
        yield return Version;
        yield return ModelVersion;
    }
}