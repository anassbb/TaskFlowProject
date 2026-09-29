using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TaskFlow.Projects.Application;

public static class ProjectsEndpoints
{
    /// <summary>Point d'entrée HTTP du module : toutes les routes vivent sous /api/projects.</summary>
    public static IEndpointRouteBuilder MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects");

        // TODO (toi) : une ligne par feature, ex.
        // CreateProject.Endpoint.Map(group);
        // GetProjects.Endpoint.Map(group);
        _ = group;

        return app;
    }
}
