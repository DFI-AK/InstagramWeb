using InstagramWeb.Application.Common.Hubs;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Interfaces.Hubs;
using InstagramWeb.Application.Common.Models;
using InstagramWeb.Application.Common.Security;
using InstagramWeb.Domain.Constants;
using InstagramWeb.Domain.Entities;
using InstagramWeb.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace InstagramWeb.Application.Chat.Commands.SendMessage;

[Authorize(Roles = Roles.User)]
public record SendMessageCommand(string ReceiverId, string Message) : IRequest<Unit>;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Receiver id is required");
    }
}

public class SendMessageCommandHandler(IApplicationDbContext context, IUser user, IHubContext<ChatHub, IChatHub> hubContext, IMapper mapper) : IRequestHandler<SendMessageCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;
    private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var (receverId, message) = request;

        var msgEntity = new UserMessage
        {
            SenderId = _user.Id,
            ReceiverId = receverId,
            TextMessage = message,
            Status = MessageStatus.Sent
        };

        await _context.Messages.AddAsync(msgEntity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var receiverMessages = await _context.Messages
            .Where(msg => (msg.SenderId == _user.Id && msg.ReceiverId == receverId) || (msg.SenderId == receverId && msg.ReceiverId == _user.Id))
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .OrderBy(x => x.Created)
            .LastOrDefaultAsync(cancellationToken: cancellationToken);

        var recMsg = _mapper.Map<MessageDto>(receiverMessages);

        await _hubContext.Clients.User(receverId).ReceiveMessage(receverId, recMsg);

        return Unit.Value;
    }
}
