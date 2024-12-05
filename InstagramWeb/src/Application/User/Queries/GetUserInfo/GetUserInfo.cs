using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.User.Queries.GetUsers;

namespace InstagramWeb.Application.User.Queries.GetUserInfo;

public record GetUserInfoQuery(string UserId) : IRequest<UserDto>;

public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User id is required.");
    }
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.UserProfiles
            .Include(x => x.Followers)
            .Include(x => x.Followed)
            .Where(x => x.Id == request.UserId)
            .AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

        var result = _mapper.Map<UserDto>(user);
        return result;
    }
}
