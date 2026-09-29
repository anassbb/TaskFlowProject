using TaskFlow.SharedKernel;

namespace TaskFlow.Projects.Domain;

/// <summary>
/// Agrégat Projet. Toutes les règles métier vivent ici : les propriétés ne sont modifiables
/// que par des méthodes qui vérifient les invariants (pas de setter public).
/// </summary>
public sealed class Project : AggregateRoot<ProjectId>
{
    public const int NameMaxLength = 100;

    private Project(ProjectId id, string name, DateTimeOffset createdAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
    }

    // Requis par EF Core.
    private Project() { }

    public string Name { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private init; }

    /// <summary>
    /// Fabrique : seul moyen de créer un projet valide. Renvoie une erreur métier au lieu de lever une exception.
    /// </summary>
    public static Result<Project> Create(string name, DateTimeOffset now)
    {
        var trimmed = name?.Trim() ?? string.Empty;

        if (trimmed.Length == 0)
        {
            return ProjectErrors.NameRequired;
        }

        if (trimmed.Length > NameMaxLength)
        {
            return ProjectErrors.NameTooLong;
        }

        var project = new Project(ProjectId.New(), trimmed, now);
        project.Raise(new ProjectCreatedDomainEvent(project.Id, project.Name));
        return project;
    }
}
