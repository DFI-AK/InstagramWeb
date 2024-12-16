using InstagramWeb.Application.Common.Models;

namespace InstagramWeb.Application.Common.Interfaces.Hubs;
public interface IChatHub
{
    Task ReceiveMessage(string receiverId, ChatBoxVm chat);
    Task SendMessage(string receiverId, ChatBoxVm chat);

    Task sentMessageSuccessfully(ChatBoxVm chts);
}
