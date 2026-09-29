using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Projects.Infrastructure.Persistence;

/// <summary>
/// DbContext propre au module Projects. Il écrit dans son propre schéma Postgres :
/// aucun autre module ne doit lire ces tables directement.
/// </summary>
public sealed class ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : DbContext(options)
{
    public const string Schema = "projects";

    // TODO (toi) : public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectsDbContext).Assembly);
    }
}
