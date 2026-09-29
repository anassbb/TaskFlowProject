using Microsoft.EntityFrameworkCore;
using TaskFlow.Projects.Application.Abstractions;
using TaskFlow.Projects.Domain;

namespace TaskFlow.Projects.Infrastructure.Persistence;

/// <remarks>Public pour la même raison que BoardRepository (code généré par Wolverine).</remarks>
public sealed class ProjectRepository(ProjectsDbContext db) : IProjectRepository
{
    public void Add(Project project) => db.Projects.Add(project);

    public async Task<IReadOnlyList<Project>> ListAsync(CancellationToken ct) =>
        await db.Projects.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
