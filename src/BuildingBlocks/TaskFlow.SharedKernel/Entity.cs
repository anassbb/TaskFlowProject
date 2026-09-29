namespace TaskFlow.SharedKernel;

/// <summary>
/// Entité : objet avec une identité qui persiste dans le temps.
/// Deux entités sont égales si elles ont le même identifiant, même si leurs autres propriétés diffèrent.
/// </summary>
/// <typeparam name="TId">Identifiant fortement typé, ex. <c>readonly record struct ProjectId(Guid Value)</c>.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : struct, IEquatable<TId>
{
    protected Entity(TId id) => Id = id;

    // Constructeur requis par EF Core pour la matérialisation.
    protected Entity() { }

    public TId Id { get; private init; }

    public bool Equals(Entity<TId>? other) =>
        other is not null && other.GetType() == GetType() && other.Id.Equals(Id);

    public override bool Equals(object? obj) => obj is Entity<TId> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => Equals(left, right);
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !Equals(left, right);
}
