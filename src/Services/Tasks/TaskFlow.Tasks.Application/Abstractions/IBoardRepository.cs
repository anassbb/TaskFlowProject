using TaskFlow.Tasks.Domain;

namespace TaskFlow.Tasks.Application.Abstractions;

/// <summary>Port de persistance des boards, implémenté par l'Infrastructure (EF Core).</summary>
public interface IBoardRepository
{
    Task<bool> ExistsForProjectAsync(Guid projectId, CancellationToken ct);

    void Add(Board board);

    Task<IReadOnlyList<Board>> ListAsync(CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}
