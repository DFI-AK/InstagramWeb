namespace InstagramWeb.Domain.Entities;
public class UserMessage : BaseEntity<string>
{
    public string? SenderId { get; set; }

    public string? ReceiverId { get; set; }

    public UserProfile Sender { get; set; } = null!;

    public UserProfile Receiver { get; set; } = null!;

    public string? TextMessage { get; set; }

    public MessageStatus Status { get; set; }
}
