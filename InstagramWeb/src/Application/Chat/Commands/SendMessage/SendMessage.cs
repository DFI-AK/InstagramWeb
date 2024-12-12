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
public record SendMessageCommand(string ReceiverId, string Message) : IRequest<Result>;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Receiver id is required");
    }
}

public class SendMessageCommandHandler(IApplicationDbContext context, IUser user, IHubContext<ChatHub, IChatHub> hubContext, IMapper mapper) : IRequestHandler<SendMessageCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;
    private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var (receverId, message) = request;

        var msgEntity = new UserMessage
        {
            SenderId = _user.Id,
            ReceiverId = receverId,
            TextMessage = message,
            Status = MessageStatus.Sent
        };
        try
        {
            await _context.Messages.AddAsync(msgEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var receiverMessages = await _context.Messages
                .Where(msg => (msg.SenderId == _user.Id && msg.ReceiverId == receverId) || (msg.SenderId == receverId && msg.ReceiverId == _user.Id))
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .OrderBy(x => x.Created)
                .LastOrDefaultAsync(cancellationToken: cancellationToken);

            var baseMsg = _mapper.Map<BaseMessageDto>(receiverMessages);

            var recMsg = new MessageDto
            {
                IsMine = baseMsg.Sender.UserId == _user.Id,
                Sender = baseMsg.Sender,
                MessageId = baseMsg.MessageId,
                MessageStatus = baseMsg.MessageStatus,
                Receiver = baseMsg.Receiver,
                SentAt = baseMsg.SentAt,
                TextMessage = baseMsg.TextMessage
            };

            await _hubContext.Clients.User(receverId).ReceiveMessage(receverId, recMsg);
            return Result.Success();
        }
        catch (HubException ex)
        {
            return Result.Failure([ex.Message]);
        }

    }
}
