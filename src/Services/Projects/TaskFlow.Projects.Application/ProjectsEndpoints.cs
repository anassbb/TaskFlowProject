using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.Projects.Application.Features;

namespace TaskFlow.Projects.Application;

public static class ProjectsEndpoints
{
    /// <summary>Point d'entrée HTTP du service : toutes les routes vivent sous /api/projects.</summary>
    public static IEndpointRouteBuilder MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects");

        // Une ligne par feature.
        CreateProject.Map(group);
        GetProjects.Map(group);

        return app;
    }
}
