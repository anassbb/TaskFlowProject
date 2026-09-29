using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

/// <summary>Utilisé UNIQUEMENT par "dotnet ef migrations add" (aucune connexion n'est ouverte).</summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TasksDbContext>
{
    public TasksDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TasksDbContext>()
            .UseNpgsql("Host=localhost;Database=tasksdb")
            .Options;

        return new TasksDbContext(options);
    }
}
