using InstagramWeb.Application.User.Queries.GetUsers;

namespace InstagramWeb.Application.Common.Models;
public class ChatBoxVm
{
    public IReadOnlyCollection<MessageDto> Messages { get; set; } = [];
    public BaseUserDto User { get; set; } = null!;
}
