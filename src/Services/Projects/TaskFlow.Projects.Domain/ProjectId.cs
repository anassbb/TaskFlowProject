namespace TaskFlow.Projects.Domain;

/// <summary>
/// Identifiant fortement typé : impossible de passer un TaskId là où on attend un ProjectId.
/// Guid v7 : triable par date de création, donc efficace comme clé d'index.
/// </summary>
public readonly record struct ProjectId(Guid Value)
{
    public static ProjectId New() => new(Guid.CreateVersion7());

    public override string ToString() => Value.ToString();
}
