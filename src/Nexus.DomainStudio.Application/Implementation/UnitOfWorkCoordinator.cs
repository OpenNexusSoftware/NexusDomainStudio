using Nexus.DomainStudio.Application.Interfaces;

namespace Nexus.DomainStudio.Application.Implementation;

/// <summary>
/// Coordinator for managing a unit of work that involves multiple participants.
/// </summary>
public sealed class UnitOfWorkCoordinator : IUnitOfWork
{
    /// <summary>
    /// List of participants enlisted in the unit of work.
    /// </summary>
    private readonly List<IUnitOfWorkParticipant> _participants = [];

    public void Enlist(IUnitOfWorkParticipant participant)
    {
        // Validate the participant before adding it to the list.
        if (participant == null)
        {
            throw new ArgumentNullException(nameof(participant), "Participant cannot be null.");
        }

        // Check if the participant is already enlisted to avoid duplicates.
        if (_participants.Contains(participant))
        {
            throw new InvalidOperationException("Participant is already enlisted.");
        }

        // Add the participant to the list of enlisted participants.
        _participants.Add(participant);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        // Check if there are any enlisted participants before proceeding.
        foreach (var p in _participants) await p.PrepareAsync(ct);

        try
        {
            // Attempt to commit changes for all enlisted participants.
            foreach (var p in _participants) await p.CommitAsync(ct);
        }
        catch
        {
            // If an error occurs during commit, roll back all participants.
            foreach (var p in _participants) await p.RollbackAsync(ct);
            throw; // Re-throw the exception to propagate it.
        }

        // Clear the list of participants after successful commit.
        _participants.Clear();
    }
}
