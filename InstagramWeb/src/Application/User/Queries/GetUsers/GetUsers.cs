using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Mappings;

namespace InstagramWeb.Application.User.Queries.GetUsers;

public record GetUsersQuery : IRequest<List<UserDto>>
{
}

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
    }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IUserServices _userServices;
    public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user,IUserServices userServices)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
        _userServices=userServices;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var baseUsers = await _context.UserProfiles
            .Where(x => x.Id != _user.Id)
            .Include(x => x.Followers)
            .Include(x => x.Followed)
            .Include(x => x.Posts)
            .OrderBy(x => x.FirstName)
            .AsNoTracking()
            .ProjectToListAsync<BaseUserDto>(_mapper.ConfigurationProvider);

        var followerUserIds = await _context.Follows
            .Where(x => x.FollowerId == _user.Id)
            .Select(x => x.FollowedId)
            .ToListAsync(cancellationToken: cancellationToken);

        var users = baseUsers
            .Select(baseUser => new UserDto
            {
                UserId = baseUser.UserId,
                FullName = baseUser.FullName,
                EmailAddress = baseUser.EmailAddress,
                ContactNumber = baseUser.ContactNumber,
                JoinAt = baseUser.JoinAt,
                IsFollowed = followerUserIds.Contains(baseUser.UserId),
                Followers = baseUser.Followers,
                Followings = baseUser.Followings
            }).ToList();

        return users;
    }
}
