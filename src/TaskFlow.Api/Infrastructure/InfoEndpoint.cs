using System.Reflection;

namespace TaskFlow.Api.Infrastructure;

/// <summary>Endpoint technique utilisé par le front pour vérifier que l'API répond.</summary>
public static class InfoEndpoint
{
    public sealed record ApiInfo(string Name, string Version, string Environment, DateTimeOffset ServerTime);

    public static IEndpointRouteBuilder MapInfoEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/info", (IHostEnvironment env) => TypedResults.Ok(new ApiInfo(
                Name: "TaskFlow API",
                Version: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0",
                Environment: env.EnvironmentName,
                ServerTime: DateTimeOffset.UtcNow)))
            .WithName("GetApiInfo")
            .WithTags("Info");

        return app;
    }
}
