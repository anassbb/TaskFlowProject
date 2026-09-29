namespace TaskFlow.Tasks.Domain;

/// <summary>Identifiant fortement typé du board (Guid v7).</summary>
public readonly record struct BoardId(Guid Value)
{
    public static BoardId New() => new(Guid.CreateVersion7());

    public override string ToString() => Value.ToString();
}
