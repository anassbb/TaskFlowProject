using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using TaskFlow.Projects.Application;
using TaskFlow.Projects.Infrastructure.Persistence;

namespace TaskFlow.Projects.Infrastructure;

/// <summary>Composition du module : l'API n'appelle que ces deux méthodes.</summary>
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

        return builder;
    }

    public static IEndpointRouteBuilder MapProjectsModule(this IEndpointRouteBuilder app) =>
        app.MapProjectsEndpoints();
}
