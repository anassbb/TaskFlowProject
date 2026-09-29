using TaskFlow.Projects.Domain;

namespace TaskFlow.Projects.Application.Abstractions;

/// <summary>
/// Port de persistance défini par l'Application, implémenté par l'Infrastructure (EF Core).
/// L'Application ne connaît donc ni EF Core ni Postgres.
/// </summary>
public interface IProjectRepository
{
    void Add(Project project);

    Task<IReadOnlyList<Project>> ListAsync(CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}
