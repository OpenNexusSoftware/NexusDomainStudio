using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;

namespace Nexus.DomainStudio.Application.Interfaces;

public interface INDSProjectDeserializer
{
    /// <summary>
    /// Deserializes the given source string into an NDSProject object.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Result<NDSProject> Deserialize(string source);
}
