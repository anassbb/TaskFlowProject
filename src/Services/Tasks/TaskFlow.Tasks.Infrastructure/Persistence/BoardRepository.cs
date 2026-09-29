using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Application.Abstractions;
using TaskFlow.Tasks.Domain;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

/// <remarks>
/// Public (et non internal) : le code que Wolverine génère pour ses handlers instancie directement
/// les dépendances. Avec un type internal, il devrait passer par le conteneur (service location),
/// ce que sa politique par défaut interdit : le message partirait en dead-letter queue.
/// </remarks>
public sealed class BoardRepository(TasksDbContext db) : IBoardRepository
{
    public Task<bool> ExistsForProjectAsync(Guid projectId, CancellationToken ct) =>
        db.Boards.AnyAsync(b => b.ProjectId == projectId, ct);

    public void Add(Board board) => db.Boards.Add(board);

    public async Task<IReadOnlyList<Board>> ListAsync(CancellationToken ct) =>
        await db.Boards.AsNoTracking().OrderByDescending(b => b.CreatedAt).ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
