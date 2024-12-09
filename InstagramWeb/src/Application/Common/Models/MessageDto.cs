using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Domain.Enums;

namespace InstagramWeb.Application.Common.Models;
public record MessageDto
{
    public string? MessageId { get; init; }
    public string? TextMessage { get; init; }
    public MessageStatus MessageStatus { get; init; }
    public DateTimeOffset SentAt { get; init; }
    public UserDto Sender { get; set; } = null!;
    public UserDto Receiver { get; set; } = null!;
}
