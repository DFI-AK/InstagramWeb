namespace InstagramWeb.Domain.Events;
public record UserCreatedEvent(string UserId, string UserName, string FirstName) : BaseEvent;
