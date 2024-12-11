using InstagramWeb.Application.Common.Models;
using InstagramWeb.Application.UserPost.Commands.CreatePost;
using InstagramWeb.Domain.Constants;

namespace InstagramWeb.Web.Endpoints;

public class UserPost : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization(x => x.RequireRole(Roles.User))
            .MapPost(Post, $"{nameof(Post)}")
            ;
    }


    public async Task<Result> Post(ISender sender, CreatePostCommand command) => await sender.Send(command);

}
