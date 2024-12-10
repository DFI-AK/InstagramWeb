using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Models;

namespace InstagramWeb.Application.User.Commands.Unfollow;

public record UnfollowCommand(string FollowedId) : IRequest<Result>;

public class UnfollowCommandValidator : AbstractValidator<UnfollowCommand>
{
    public UnfollowCommandValidator()
    {
        RuleFor(x => x.FollowedId).NotEmpty().WithMessage("Followed id is required");
    }
}

public class UnfollowCommandHandler : IRequestHandler<UnfollowCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _follower;

    public UnfollowCommandHandler(IApplicationDbContext context, IUser follower)
    {
        _context = context;
        _follower = follower;
    }

    public async Task<Result> Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        var following = await _context.Follows.FirstOrDefaultAsync(x => x.FollowedId == request.FollowedId && x.FollowerId == _follower.Id);
        if (following == null) return Result.Failure(["User not found."]);
        _context.Follows.Remove(following);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
