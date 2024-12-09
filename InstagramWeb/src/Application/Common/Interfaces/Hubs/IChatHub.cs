using InstagramWeb.Application.Common.Models;

namespace InstagramWeb.Application.Common.Interfaces.Hubs;
public interface IChatHub
{
    Task ReceiveMessage(string receiverId, MessageDto message);
    Task SendMessage(string receiverId, List<MessageDto> messages);
}
