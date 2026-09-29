using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Tasks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTasksApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        // Les handlers d'événements (ProjectCreatedHandler...) sont découverts et construits par Wolverine.
        return services;
    }
}
