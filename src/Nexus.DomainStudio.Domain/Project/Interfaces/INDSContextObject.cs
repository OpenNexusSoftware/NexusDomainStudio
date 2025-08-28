using Nexus.DomainStudio.Domain.Project.Enums;

namespace Nexus.DomainStudio.Domain.Project.Interfaces;

/// <summary>
/// Marker interface for all NDS (Nexus Domain Studio) objects that have an Id.
/// </summary>
public interface INDSContextObject
{
    string Id { get; }
    NDSObjectType ObjectType { get; }
}
