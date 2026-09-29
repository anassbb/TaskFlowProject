using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

/// <summary>DbContext propre au service Tasks, dans sa propre base (tasksdb) et son propre schéma.</summary>
public sealed class TasksDbContext(DbContextOptions<TasksDbContext> options) : DbContext(options)
{
    public const string Schema = "tasks";

    public DbSet<Board> Boards => Set<Board>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TasksDbContext).Assembly);
    }
}
