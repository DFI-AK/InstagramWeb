using InstagramWeb.Application.Common.Hubs;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Interfaces.Hubs;
using InstagramWeb.Application.Common.Mappings;
using InstagramWeb.Application.Common.Models;
using Microsoft.AspNetCore.SignalR;

namespace InstagramWeb.Application.Chat.Queries.ReceveMessage;

public record ReceveMessageQuery(string SenderId) : IRequest<Unit>;

public class ReceveMessageQueryValidator : AbstractValidator<ReceveMessageQuery>
{
    public ReceveMessageQueryValidator()
    {
        RuleFor(x => x.SenderId).NotEmpty().WithMessage("Sender id should not be empty.");
    }
}

public class ReceveMessageQueryHandler(IApplicationDbContext context, IHubContext<ChatHub, IChatHub> hubContext, IUser receiver, IMapper mapper) : IRequestHandler<ReceveMessageQuery, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
    private readonly IUser _receiver = receiver;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(ReceveMessageQuery request, CancellationToken cancellationToken)
    {
        var msg = await _context.Messages
            .OrderBy(x => x.Created)
            .Include(x => x.Sender)
            .Include(x => x.Receiver)
            .Where(x => (x.SenderId == request.SenderId && x.ReceiverId == _receiver.Id) || (x.ReceiverId == request.SenderId && x.SenderId == _receiver.Id))
            .AsNoTracking()
            .ProjectToListAsync<BaseMessageDto>(_mapper.ConfigurationProvider);

        List<MessageDto> msgDto = msg.Select(rec => new MessageDto
        {
            IsMine = rec.Sender.UserId == _receiver.Id,
            Sender = rec.Sender,
            MessageId = rec.MessageId,
            MessageStatus = rec.MessageStatus,
            Receiver = rec.Receiver,
            SentAt = rec.SentAt,
            TextMessage = rec.TextMessage
        }).ToList();

        await _hubContext.Clients.User(_receiver.Id ?? string.Empty).SendMessage(_receiver.Id ?? string.Empty, msgDto);

        return Unit.Value;
    }
}
