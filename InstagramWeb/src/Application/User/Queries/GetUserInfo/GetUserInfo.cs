using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Domain.Enums;

namespace InstagramWeb.Application.User.Queries.GetUserInfo;

public record GetUserInfoQuery : IRequest<UserProfileVm>;

public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {

    }
}

public class GetUserInfoQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user) : IRequestHandler<GetUserInfoQuery, UserProfileVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IUser _user = user;

    public async Task<UserProfileVm> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == _user.Id, cancellationToken: cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);

        return new()
        {
            PostCategories = Enum.GetValues<PostCatergory>()
            .Cast<PostCatergory>()
            .Select(rec => new PostCategoryDto { Id = (int)rec, Title = rec.ToString() })
            .ToList(),
            UserProfile = userDto
        };
    }
}
