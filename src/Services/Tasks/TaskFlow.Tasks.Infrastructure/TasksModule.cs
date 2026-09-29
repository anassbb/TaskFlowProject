using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Tasks.Application;
using TaskFlow.Tasks.Application.Abstractions;
using TaskFlow.Tasks.Infrastructure.Persistence;

namespace TaskFlow.Tasks.Infrastructure;

/// <summary>Composition du service Tasks.</summary>
public static class TasksModule
{
    /// <summary>Base propre au service Tasks, déclarée dans l'AppHost (<c>AddDatabase("tasksdb")</c>).</summary>
    public const string DatabaseName = "tasksdb";

    public static IHostApplicationBuilder AddTasksModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTasksApplication();

        builder.AddNpgsqlDbContext<TasksDbContext>(DatabaseName);

        builder.Services.AddScoped<IBoardRepository, BoardRepository>();
        builder.Services.AddSingleton(TimeProvider.System);

        return builder;
    }

    public static IEndpointRouteBuilder MapTasksModule(this IEndpointRouteBuilder app) =>
        app.MapTasksEndpoints();

    /// <summary>Applique les migrations au démarrage (développement uniquement).</summary>
    public static async Task MigrateTasksDatabaseAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
        await db.Database.MigrateAsync();
    }
}
