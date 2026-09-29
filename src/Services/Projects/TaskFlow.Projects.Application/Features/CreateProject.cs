using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskFlow.Api.Common;
using TaskFlow.Projects.Application.Abstractions;
using TaskFlow.Projects.Contracts;
using TaskFlow.Projects.Domain;
using TaskFlow.SharedKernel;
using Wolverine;

namespace TaskFlow.Projects.Application.Features;

/// <summary>
/// Vertical slice « créer un projet » : requête, validation, traitement et endpoint au même endroit.
/// POST /api/projects  { "name": "Refonte du site" }
/// </summary>
public static class CreateProject
{
    public sealed record Command(string Name);

    public sealed record Response(Guid Id, string Name, DateTimeOffset CreatedAt);

    /// <summary>Validation de FORME (champ présent, longueur). Les règles MÉTIER restent dans l'agrégat.</summary>
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(Project.NameMaxLength);
        }
    }

    public sealed class Handler(IProjectRepository projects, IMessageBus bus, TimeProvider clock)
    {
        public async Task<Result<Response>> Handle(Command command, CancellationToken ct)
        {
            var created = Project.Create(command.Name, clock.GetUtcNow());
            if (created.IsFailure)
            {
                return created.Error;
            }

            var project = created.Value;
            projects.Add(project);
            await projects.SaveChangesAsync(ct);

            // Informe les autres services (Tasks créera le board du projet).
            // ⚠ Sans outbox, un crash entre SaveChanges et PublishAsync perdrait l'événement :
            //   c'est exactement ce que l'étape 3 corrige (outbox Wolverine, même transaction).
            await bus.PublishAsync(new ProjectCreated(project.Id.Value, project.Name, project.CreatedAt));

            return new Response(project.Id.Value, project.Name, project.CreatedAt);
        }
    }

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", async Task<IResult> (Command command, IValidator<Command> validator, Handler handler, CancellationToken ct) =>
            {
                if (await validator.ValidateRequestAsync(command, ct) is { } invalid)
                {
                    return invalid;
                }

                var result = await handler.Handle(command, ct);
                return result.IsSuccess
                    ? TypedResults.Created($"/api/projects/{result.Value.Id}", result.Value)
                    : result.ToProblem();
            })
            .WithName("CreateProject")
            .WithSummary("Crée un projet et publie l'événement ProjectCreated sur RabbitMQ.");
}
