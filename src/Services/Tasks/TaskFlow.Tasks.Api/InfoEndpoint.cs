using System.Reflection;

namespace TaskFlow.Tasks.Api;

/// <summary>Endpoint technique : identité et version du service Tasks (exposé via la Gateway).</summary>
public static class InfoEndpoint
{
    public sealed record ServiceInfo(string Name, string Version, string Environment, DateTimeOffset ServerTime);

    public static IEndpointRouteBuilder MapInfoEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tasks/info", (IHostEnvironment env) => TypedResults.Ok(new ServiceInfo(
                Name: "TaskFlow Tasks",
                Version: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0",
                Environment: env.EnvironmentName,
                ServerTime: DateTimeOffset.UtcNow)))
            .WithName("GetTasksServiceInfo")
            .WithTags("Info");

        return app;
    }
}
