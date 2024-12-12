using InstagramWeb.Application.Common.Hubs;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Interfaces.Hubs;
using InstagramWeb.Application.Common.Mappings;
using InstagramWeb.Application.Common.Models;
using InstagramWeb.Application.User.Queries.GetUsers;
using Microsoft.AspNetCore.SignalR;

namespace InstagramWeb.Application.Chat.Queries.ReceveMessage;

public record ReceveMessageQuery(string ReceiverId) : IRequest<Unit>;

public class ReceveMessageQueryValidator : AbstractValidator<ReceveMessageQuery>
{
    public ReceveMessageQueryValidator()
    {
        RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Sender id should not be empty.");
    }
}

public class ReceveMessageQueryHandler(IApplicationDbContext context, IHubContext<ChatHub, IChatHub> hubContext, IUser user, IMapper mapper) : IRequestHandler<ReceveMessageQuery, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
    private readonly IUser _user = user;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(ReceveMessageQuery request, CancellationToken cancellationToken)
    {
        var msg = await _context.Messages
            .Where(x => (x.ReceiverId == request.ReceiverId && x.ReceiverId == _user.Id) || (x.ReceiverId == request.ReceiverId && x.ReceiverId == _user.Id))
            .OrderBy(x => x.Created)
            .Include(x => x.Sender)
            .Include(x => x.Receiver)
            .Where(x => (x.ReceiverId == request.ReceiverId && x.ReceiverId == _user.Id) || (x.ReceiverId == request.ReceiverId && x.ReceiverId == _user.Id))
            .AsNoTracking()
            .ProjectToListAsync<BaseMessageDto>(_mapper.ConfigurationProvider);

        List<MessageDto> msgDto = msg.Select(rec => new MessageDto
        {
            IsMine = rec.Sender.UserId == _user.Id,
            Sender = rec.Sender,
            MessageId = rec.MessageId,
            MessageStatus = rec.MessageStatus,
            Receiver = rec.Receiver,
            SentAt = rec.SentAt,
            TextMessage = rec.TextMessage
        }).ToList();

        var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == request.ReceiverId, cancellationToken: cancellationToken);

        var userDto = _mapper.Map<BaseUserDto>(user);

        var chatDto = new ChatBoxVm
        {
            Messages = msgDto,
            User = userDto
        };

        await _hubContext.Clients.User(_user.Id ?? string.Empty).SendMessage(_user.Id ?? string.Empty, chatDto);

        return Unit.Value;
    }
}
