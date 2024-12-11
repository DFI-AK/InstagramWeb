using InstagramWeb.Application.Common.Interfaces;

namespace InstagramWeb.Application.Chat.Queries.GetMessages;

public record GetMessagesQuery : IRequest<List<GetMessageDto>>
{
    public string UserId { get; set; } = string.Empty;
}

public class GetMessagesQueryValidator : AbstractValidator<GetMessagesQuery>
{
    public GetMessagesQueryValidator()
    {
    }
}

public class GetMessagesQueryHandler(IMessageServices messageservice) : IRequestHandler<GetMessagesQuery, List<GetMessageDto>>
{
    private readonly IMessageServices _messageServices=messageservice;

    public async Task<List<GetMessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _messageServices.GetAllMessages(request.UserId);
        }
        catch (Exception ) { 

         return new List<GetMessageDto>();
        }
    }
}
