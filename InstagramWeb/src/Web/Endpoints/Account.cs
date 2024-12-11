
using InstagramWeb.Infrastructure.Identity;

namespace InstagramWeb.Web.Endpoints;

public class Account : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapIdentityApi<ApplicationUser>();
    }
}
