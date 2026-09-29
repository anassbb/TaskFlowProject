using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Projects.Application;
using TaskFlow.Projects.Application.Abstractions;
using TaskFlow.Projects.Infrastructure.Persistence;

namespace TaskFlow.Projects.Infrastructure;

/// <summary>Composition du service : l'API n'appelle que ces méthodes.</summary>
public static class ProjectsModule
{
    /// <summary>Base propre au service Projects, déclarée dans l'AppHost (<c>AddDatabase("projectsdb")</c>).</summary>
    public const string DatabaseName = "projectsdb";

    public static IHostApplicationBuilder AddProjectsModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProjectsApplication();

        // Intégration Aspire : lit la chaîne de connexion injectée par l'AppHost,
        // et ajoute retries, health check et traces OpenTelemetry pour EF Core.
        builder.AddNpgsqlDbContext<ProjectsDbContext>(DatabaseName);

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddSingleton(TimeProvider.System);

        return builder;
    }

    public static IEndpointRouteBuilder MapProjectsModule(this IEndpointRouteBuilder app) =>
        app.MapProjectsEndpoints();

    /// <summary>
    /// Applique les migrations EF Core au démarrage. Réservé au DÉVELOPPEMENT :
    /// en production, les migrations passent par un bundle exécuté par la CI/CD.
    /// </summary>
    public static async Task MigrateProjectsDatabaseAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ProjectsDbContext>();
        await db.Database.MigrateAsync();
    }
}
