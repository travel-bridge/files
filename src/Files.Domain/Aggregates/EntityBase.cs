using MediatR;

namespace Files.Domain.Aggregates;

public class EntityBase<TId> : IWithDomainEvents
    where TId : IEquatable<TId>
{
    public TId Id { get; protected init; } = default!;
    
    private readonly List<INotification> _domainEvents = new();
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
    
    public void AddDomainEvent(INotification @event) => _domainEvents.Add(@event);

    public void RemoveDomainEvent(INotification @event) => _domainEvents.Remove(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public bool IsTransient() => Id.Equals(default);

    public override bool Equals(object? obj)
    {
        if (obj is not EntityBase<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (other.IsTransient() || IsTransient())
            return false;
        
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        if (IsTransient())
            return base.GetHashCode();
        
        return EqualityComparer<TId>.Default.GetHashCode(Id);
    }

    public static bool operator ==(EntityBase<TId>? left, EntityBase<TId>? right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    public static bool operator !=(EntityBase<TId>? left, EntityBase<TId>? right)
    {
        return !(left == right);
    }
}