using Nexus.DomainStudio.Domain.Project.Enums;

namespace Nexus.DomainStudio.Domain.Project.Interfaces;

/// <summary>
/// Marker interface for all NDS (Nexus Domain Studio) objects that have an Id.
/// </summary>
public interface INDSContextObject
{
    /// <summary>
    /// The unique identifier of the NDS object.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The type of the NDS object.
    /// </summary>
    NDSObjectType ObjectType { get; }
}
