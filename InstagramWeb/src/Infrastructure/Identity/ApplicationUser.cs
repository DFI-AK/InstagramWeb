using System.ComponentModel.DataAnnotations.Schema;
using InstagramWeb.Domain.Common;
using InstagramWeb.Domain.Common.Interfaces;
using InstagramWeb.Domain.Entities;
using InstagramWeb.Domain.Events;
using Microsoft.AspNetCore.Identity;

namespace InstagramWeb.Infrastructure.Identity;
public class ApplicationUser : IdentityUser, IDomain
{
    public override string? UserName
    {
        get => base.UserName;
        set
        {
            base.UserName = value;
            if (!string.IsNullOrEmpty(base.UserName))
            {
                string firstName = base.UserName.Split('@')[0].ToUpper();
                AddDomainEvent(new UserCreatedEvent(Id, base.UserName, firstName));
            }
        }
    }

    public UserProfile UserProfile { get; set; } = null!;

    private List<BaseEvent> _domainEvents { get; set; } = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

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
