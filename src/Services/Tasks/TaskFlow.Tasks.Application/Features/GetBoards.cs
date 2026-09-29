using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.Tasks.Application.Abstractions;

namespace TaskFlow.Tasks.Application.Features;

/// <summary>GET /api/tasks/boards : liste des boards (un par projet).</summary>
public static class GetBoards
{
    public sealed record Item(Guid Id, Guid ProjectId, string ProjectName, IReadOnlyList<string> Columns, DateTimeOffset CreatedAt);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/boards", async (IBoardRepository boards, CancellationToken ct) =>
            {
                var list = await boards.ListAsync(ct);
                return TypedResults.Ok(list
                    .Select(b => new Item(b.Id.Value, b.ProjectId, b.ProjectName, b.Columns, b.CreatedAt))
                    .ToList());
            })
            .WithName("GetBoards");
}
