using InstagramWeb.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace InstagramWeb.Application.User.Queries.isFollow;

public record isFollowQuery : IRequest<bool>
{
    public string followedId { get; set; } = string.Empty;
}

public class isFollowQueryValidator : AbstractValidator<isFollowQuery>
{
    public isFollowQueryValidator()
    {
    }
}

public class isFollowQueryHandler(IApplicationDbContext context, ILogger<isFollowQueryHandler> logger, IUser user) : IRequestHandler<isFollowQuery, bool>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ILogger<isFollowQueryHandler> _logger = logger;
    private readonly IUser _user = user;

    public async Task<bool> Handle(isFollowQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Follows.FirstOrDefaultAsync(x => x.FollowerId == _user.Id && x.FollowedId == request.followedId);

            if (user == null)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }
}
