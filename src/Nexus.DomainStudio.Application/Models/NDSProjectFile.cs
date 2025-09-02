using Nexus.DomainStudio.Domain.Project.Entities;

namespace Nexus.DomainStudio.Application.Models;

/// <summary>
/// Represents a Nexus Domain Studio project file.
/// </summary>
public class NDSProjectFile
{
    private readonly string _filePath;
    private readonly string _content;

    private readonly NDSProject _ndsProject;

    /// <summary>
    /// Creates a new instance of the <see cref="NDSProjectFile"/> class.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    /// <param name="ndsProject"></param>
    public NDSProjectFile(string filePath, string content, NDSProject ndsProject)
    {
        _filePath = filePath;
        _content = content;
        _ndsProject = ndsProject;
    }

    /// <summary>
    /// Gets the content of the project file.
    /// </summary>
    public string FileContent => _content;

    /// <summary>
    /// Gets the file path of the project file.
    /// </summary>
    public string FilePath => _filePath;

    /// <summary>
    /// Gets the NDS project associated with this file.
    /// </summary>
    public NDSProject Project => _ndsProject;
}
