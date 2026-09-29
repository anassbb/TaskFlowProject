using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Projects.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectsApplication(this IServiceCollection services)
    {
        // Enregistre automatiquement tous les AbstractValidator<T> du module.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        // TODO (toi) : enregistrer ici les handlers de tes features, ex.
        // services.AddScoped<CreateProject.Handler>();
        return services;
    }
}
