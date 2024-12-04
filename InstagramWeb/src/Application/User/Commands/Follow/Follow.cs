using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Models;

namespace InstagramWeb.Application.User.Commands.Follow;

public record FollowCommand(string FollowedId) : IRequest<Result>;

public class FollowCommandValidator : AbstractValidator<FollowCommand>
{
    public FollowCommandValidator()
    {
        RuleFor(x => x.FollowedId).NotEmpty().WithMessage(x => $"{nameof(x.FollowedId)} is requried.");
    }
}

public class FollowCommandHandler : IRequestHandler<FollowCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public FollowCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Result> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.FollowedId))
            {
                throw new ArgumentNullException("User id is null");
            }

            await _context.Follows.AddAsync(new()
            {
                FollowedId = request.FollowedId,
                FollowerId = _user.Id
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (ArgumentNullException ex)
        {
            return Result.Failure([ex.Message]);
        }
    }
}
