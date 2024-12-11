
using InstagramWeb.Application.Common.Models;
using InstagramWeb.Application.User.Commands.Follow;
using InstagramWeb.Application.User.Queries.getFollowers;
using InstagramWeb.Application.User.Commands.Unfollow;
using InstagramWeb.Application.User.Queries.GetUserInfo;
using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Application.User.Queries.isFollow;
using InstagramWeb.Domain.Constants;

namespace InstagramWeb.Web.Endpoints;

public class User : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization(x => x.RequireRole(Roles.User))
            .MapGet(GetUsers, $"{nameof(GetUsers)}")
            .MapGet(GetUserInfo, $"{nameof(GetUserInfo)}")
            .MapPost(Follow, $"{nameof(Follow)}")
            .MapPost(Unfollow, $"{nameof(Unfollow)}")
            .MapGet(isFollow, $"{nameof(isFollow)}")
            .MapGet(getFollowers,$"{nameof(getFollowers)}");
    }

    public async Task<List<UserDto>> GetUsers(ISender sender) => await sender.Send(new GetUsersQuery());

    public async Task<List<FollowersDtos>> getFollowers(ISender sender) => await sender.Send(new getFollowersQuery());

    public async Task<bool> isFollow(ISender sender,string UserId)=> await sender.Send(new isFollowQuery(){ followedId=UserId });

    public async Task<Result> Follow(ISender sender, FollowCommand command) => await sender.Send(command);

    public async Task<Result> Unfollow(ISender sender, UnfollowCommand command) => await sender.Send(command);

    public async Task<UserProfileVm> GetUserInfo(ISender sender) => await sender.Send(new GetUserInfoQuery());

}
