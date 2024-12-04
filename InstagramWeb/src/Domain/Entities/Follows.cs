using System.ComponentModel.DataAnnotations.Schema;
using InstagramWeb.Domain.Common.Interfaces;

namespace InstagramWeb.Domain.Entities;
public class Follows : IAuditableEntity, IDomain
{
    public string? FollowerId { get; set; }

    public string? FollowedId { get; set; }

    public UserProfile Follower { get; set; } = null!;

    public UserProfile Followed { get; set; } = null!;

    private List<BaseEvent> _domainEvents { get; set; } = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public void AddDomainEvent(BaseEvent baseEvent)
    {
        _domainEvents.Add(baseEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void RemoveDomainEvent(BaseEvent baseEvent)
    {
        _domainEvents.Remove(baseEvent);
    }
}
