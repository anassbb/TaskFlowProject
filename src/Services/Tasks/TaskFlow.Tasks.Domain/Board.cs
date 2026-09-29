using TaskFlow.SharedKernel;

namespace TaskFlow.Tasks.Domain;

/// <summary>
/// Board Kanban d'un projet. Un projet a exactement un board, créé automatiquement
/// quand le service Tasks reçoit l'événement ProjectCreated.
/// </summary>
public sealed class Board : AggregateRoot<BoardId>
{
    public static readonly IReadOnlyList<string> DefaultColumns = ["À faire", "En cours", "Terminé"];

    private readonly List<string> _columns = [];

    private Board(BoardId id, Guid projectId, string projectName, DateTimeOffset createdAt) : base(id)
    {
        ProjectId = projectId;
        ProjectName = projectName;
        CreatedAt = createdAt;
        _columns.AddRange(DefaultColumns);
    }

    // Requis par EF Core.
    private Board() { }

    /// <summary>
    /// Référence au projet d'un AUTRE service : un simple Guid, jamais un objet du domaine Projects.
    /// </summary>
    public Guid ProjectId { get; private init; }

    /// <summary>Copie locale du nom du projet (cohérence éventuelle : mise à jour par événement).</summary>
    public string ProjectName { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private init; }

    public IReadOnlyList<string> Columns => _columns.AsReadOnly();

    public static Board CreateForProject(Guid projectId, string projectName, DateTimeOffset now) =>
        new(BoardId.New(), projectId, projectName, now);
}
