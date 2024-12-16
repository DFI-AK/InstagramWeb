using InstagramWeb.Application.Common.Models;
using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Application.UserPost.Commands.CreatePost;
using InstagramWeb.Domain.Constants;

namespace InstagramWeb.Web.Endpoints;

public class UserPost : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization(x => x.RequireRole(Roles.User))
             .MapGet(GetAllUsersPosts, $"{nameof(GetAllUsersPosts)}")
            .MapPost(Post, $"{nameof(Post)}")
            ;
    }


    public async Task<Result> Post(ISender sender, CreatePostCommand command) => await sender.Send(command);

    public async Task<List<Application.User.Queries.GetUsers.UserPost>> GetAllUsersPosts(ISender sender) => await sender.Send(new GetUsersPostsQuery());


}
