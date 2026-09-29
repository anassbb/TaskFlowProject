using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Projects.Application.Features;

namespace TaskFlow.Projects.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectsApplication(this IServiceCollection services)
    {
        // Enregistre automatiquement tous les AbstractValidator<T> du module.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        // Un handler par feature qui modifie l'état.
        services.AddScoped<CreateProject.Handler>();

        return services;
    }
}
