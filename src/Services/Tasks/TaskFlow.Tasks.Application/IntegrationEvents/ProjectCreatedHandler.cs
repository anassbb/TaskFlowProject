using Microsoft.Extensions.Logging;
using TaskFlow.Projects.Contracts;
using TaskFlow.Tasks.Application.Abstractions;
using TaskFlow.Tasks.Domain;

namespace TaskFlow.Tasks.Application.IntegrationEvents;

/// <summary>
/// Réagit à l'événement ProjectCreated publié par le service Projects : crée le board du projet.
/// Wolverine le découvre par convention (classe "...Handler", méthode "Handle") ; aucune dépendance à Wolverine ici.
/// </summary>
public sealed partial class ProjectCreatedHandler(
    IBoardRepository boards,
    TimeProvider clock,
    ILogger<ProjectCreatedHandler> logger)
{
    public async Task Handle(ProjectCreated message, CancellationToken ct)
    {
        // IDEMPOTENCE : RabbitMQ garantit "au moins une fois", le même message peut arriver deux fois.
        // Rejouer ce handler ne doit jamais créer un second board.
        if (await boards.ExistsForProjectAsync(message.ProjectId, ct))
        {
            LogAlreadyExists(logger, message.ProjectId);
            return;
        }

        boards.Add(Board.CreateForProject(message.ProjectId, message.Name, clock.GetUtcNow()));
        await boards.SaveChangesAsync(ct);

        LogBoardCreated(logger, message.ProjectId, message.Name);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Board créé pour le projet {ProjectId} ({ProjectName})")]
    private static partial void LogBoardCreated(ILogger logger, Guid projectId, string projectName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Board déjà existant pour le projet {ProjectId} : message ignoré (idempotence)")]
    private static partial void LogAlreadyExists(ILogger logger, Guid projectId);
}
