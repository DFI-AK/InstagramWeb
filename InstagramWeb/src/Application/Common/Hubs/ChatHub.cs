using InstagramWeb.Application.Chat.Commands.SendMessage;
using InstagramWeb.Application.Chat.Queries.ReceveMessage;
using InstagramWeb.Application.Common.Interfaces.Hubs;
using InstagramWeb.Application.Common.Security;
using InstagramWeb.Domain.Constants;
using Microsoft.AspNetCore.SignalR;

namespace InstagramWeb.Application.Common.Hubs;
[Authorize(Roles = Roles.User)]
public class ChatHub(ISender sender) : Hub<IChatHub>
{
    private readonly ISender _sender = sender;

    public async Task InvokeSendMessage(string receiverId, string message)
    {
        var request = new SendMessageCommand(receiverId, message);
        await _sender.Send(request);
    }

    public async Task InvokeReceiveMessage(string senderId)
    {
        var request = new ReceveMessageQuery(senderId);
        await _sender.Send(request);
    }
}
