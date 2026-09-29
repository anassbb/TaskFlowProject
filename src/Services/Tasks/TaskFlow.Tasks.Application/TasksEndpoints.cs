using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.Tasks.Application.Features;

namespace TaskFlow.Tasks.Application;

public static class TasksEndpoints
{
    /// <summary>Point d'entrée HTTP du service : toutes les routes vivent sous /api/tasks.</summary>
    public static IEndpointRouteBuilder MapTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");

        GetBoards.Map(group);

        return app;
    }
}
