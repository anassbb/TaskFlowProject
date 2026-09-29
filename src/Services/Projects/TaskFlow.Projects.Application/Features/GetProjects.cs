using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.Projects.Application.Abstractions;

namespace TaskFlow.Projects.Application.Features;

/// <summary>GET /api/projects : liste des projets (du plus récent au plus ancien).</summary>
public static class GetProjects
{
    public sealed record Item(Guid Id, string Name, DateTimeOffset CreatedAt);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/", async (IProjectRepository projects, CancellationToken ct) =>
            {
                var list = await projects.ListAsync(ct);
                return TypedResults.Ok(list.Select(p => new Item(p.Id.Value, p.Name, p.CreatedAt)).ToList());
            })
            .WithName("GetProjects");
}
