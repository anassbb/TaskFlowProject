using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.Projects.Infrastructure.Persistence;

/// <summary>
/// Utilisé UNIQUEMENT par "dotnet ef migrations add" : fournit un DbContext sans démarrer l'API
/// (dont la chaîne de connexion n'existe que lorsqu'Aspire la lance). Aucune connexion n'est ouverte.
/// </summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectsDbContext>
{
    public ProjectsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ProjectsDbContext>()
            .UseNpgsql("Host=localhost;Database=projectsdb")
            .Options;

        return new ProjectsDbContext(options);
    }
}
