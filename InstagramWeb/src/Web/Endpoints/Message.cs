
using InstagramWeb.Application.Chat.Queries.GetMessages;
using InstagramWeb.Domain.Constants;

namespace InstagramWeb.Web.Endpoints;

public class Message : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetMessages, $"{nameof(GetMessages)}/{{userId}}");
    }

    public async Task<List<GetMessageDto>> GetMessages(ISender sender, string userId) => await sender.Send(new GetMessagesQuery
    {
        UserId = userId
    });
}
