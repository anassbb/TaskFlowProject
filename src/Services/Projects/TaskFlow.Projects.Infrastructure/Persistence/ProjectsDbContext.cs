using Microsoft.EntityFrameworkCore;
using TaskFlow.Projects.Domain;

namespace TaskFlow.Projects.Infrastructure.Persistence;

/// <summary>
/// DbContext propre au service Projects, dans sa propre base (projectsdb) et son propre schéma.
/// Aucun autre service ne lit ces tables : ils reçoivent les événements à la place.
/// </summary>
public sealed class ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : DbContext(options)
{
    public const string Schema = "projects";

    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectsDbContext).Assembly);
    }
}
