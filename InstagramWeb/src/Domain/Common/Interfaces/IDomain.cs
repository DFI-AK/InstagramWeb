namespace InstagramWeb.Domain.Common.Interfaces;
public interface IDomain
{
    IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    void AddDomainEvent(BaseEvent baseEvent);

    void RemoveDomainEvent(BaseEvent baseEvent);

    void ClearDomainEvents();
}
