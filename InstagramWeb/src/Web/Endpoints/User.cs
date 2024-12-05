
using InstagramWeb.Application.Common.Models;
using InstagramWeb.Application.User.Commands.Follow;
using InstagramWeb.Application.User.Queries.GetUserInfo;
using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Domain.Constants;

namespace InstagramWeb.Web.Endpoints;

public class User : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization(x => x.RequireRole(Roles.User))
            .MapGet(GetUsers, $"{nameof(GetUsers)}")
            .MapGet(GetUserInfo, $"{nameof(GetUserInfo)}/{{userId}}")
            .MapPost(Follow, $"{nameof(Follow)}")
            ;
    }

    public async Task<List<UserDto>> GetUsers(ISender sender) => await sender.Send(new GetUsersQuery());

    public async Task<Result> Follow(ISender sender, FollowCommand command) => await sender.Send(command);

    public async Task<UserDto> GetUserInfo(ISender sender, string userId) => await sender.Send(new GetUserInfoQuery(userId));
}
