using System.Reflection;

namespace TaskFlow.Projects.Api.Infrastructure;

/// <summary>Endpoint technique : identité et version du service Projects (exposé via la Gateway).</summary>
public static class InfoEndpoint
{
    public sealed record ServiceInfo(string Name, string Version, string Environment, DateTimeOffset ServerTime);

    public static IEndpointRouteBuilder MapInfoEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/projects/info", (IHostEnvironment env) => TypedResults.Ok(new ServiceInfo(
                Name: "TaskFlow Projects",
                Version: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0",
                Environment: env.EnvironmentName,
                ServerTime: DateTimeOffset.UtcNow)))
            .WithName("GetProjectsServiceInfo")
            .WithTags("Info");

        return app;
    }
}
