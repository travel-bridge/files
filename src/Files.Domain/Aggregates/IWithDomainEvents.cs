using MediatR;

namespace Files.Domain.Aggregates;

public interface IWithDomainEvents
{
    IReadOnlyCollection<INotification> DomainEvents { get; }
    
    void AddDomainEvent(INotification @event);

    void RemoveDomainEvent(INotification @event);

    void ClearDomainEvents();
}