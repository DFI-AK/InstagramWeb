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

    public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.UserProfiles
            .Where(x => x.Id != _user.Id)
            .Include(x => x.Followers)
            .Include(x => x.Followed)
            .OrderBy(x => x.FirstName)
            .AsNoTracking()
            .ProjectToListAsync<UserDto>(_mapper.ConfigurationProvider);
        return users;
    }
}
