namespace TaskFlow.SharedKernel;

/// <summary>
/// Événement métier levé par un agrégat (ex. <c>ProjectCreated</c>).
/// Il est collecté par l'agrégat puis publié au moment du SaveChanges.
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAt { get; }
}

/// <summary>Base pratique : <c>public sealed record ProjectCreated(ProjectId Id) : DomainEvent;</c></summary>
public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.CreateVersion7();
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
