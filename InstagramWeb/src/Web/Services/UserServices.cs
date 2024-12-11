using AutoMapper;
using Azure.Core;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Mappings;
using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Web.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace InstagramWeb.Web.Services;

public class UserServices : IUserServices
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public UserServices(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    async Task<bool> isFollowed(string userID)
    {
        try
        {
            var user = await _context.Follows.FirstOrDefaultAsync(x => x.FollowerId == _user.Id && x.FollowedId == userID);

            if (user == null)
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<List<UserDto>> getUsersInfo()
    {
        var users = await _context.UserProfiles
           .Where(x => x.Id != _user.Id)
           .Include(x => x.Followers)
           .Include(x => x.Followed)
           .OrderBy(x => x.FirstName)
           .AsNoTracking()
           .ProjectToListAsync<UserDto>(_mapper.ConfigurationProvider);

        foreach (var user in users)
        {
            user.IsFollowed =await isFollowed(user.UserId??"");
        }
        return users;
    }
}
