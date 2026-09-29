namespace TaskFlow.Projects.Contracts;

/// <summary>
/// Événement d'intégration publié sur RabbitMQ quand un projet est créé.
/// Contrat public : types simples uniquement, et on n'enlève ni ne renomme jamais un champ.
/// </summary>
/// <param name="ProjectId">Identifiant du projet (le même dans tous les services).</param>
/// <param name="Name">Nom du projet au moment de sa création.</param>
/// <param name="CreatedAt">Date de création, en UTC.</param>
public sealed record ProjectCreated(Guid ProjectId, string Name, DateTimeOffset CreatedAt);
