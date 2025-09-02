using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Entities;

namespace Nexus.DomainStudio.Application.Interfaces;

/// <summary>
/// Deserializes NDSContext from a given source string.
/// </summary>
public interface INDSContextDeserializer
{
    public Result<NDSContext> Deserialize(string source);
}
