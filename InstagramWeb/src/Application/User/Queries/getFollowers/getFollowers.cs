using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Mappings;
using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.User.Queries.getFollowers;

public record getFollowersQuery : IRequest<List<FollowersDtos>>
{
}

public class getFollowersQueryValidator : AbstractValidator<getFollowersQuery>
{
    public getFollowersQueryValidator()
    {
    }
}

public class getFollowersQueryHandler(IApplicationDbContext context, IUser user, IMapper mapper) : IRequestHandler<getFollowersQuery, List<FollowersDtos>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;
    private readonly IMapper _mapper = mapper;

    public async Task<List<FollowersDtos>> Handle(getFollowersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.UserProfiles
                .Include(x => x.Followers)
                .Include(x => x.Followed)
                .FirstOrDefaultAsync(x => x.Id == _user.Id);

            var joined = user?.Followed.Select(x => new FollowersDtos { 
            userId=x.FollowerId??"",
            
            }).ToList();

            return joined ?? [];
        }
        catch
        {
            return [];
        }
    }
}
