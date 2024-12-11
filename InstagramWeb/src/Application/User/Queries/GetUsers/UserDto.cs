using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.User.Queries.GetUsers;
public record BaseUserDto
{
    public string? UserId { get; init; }
    public string? FullName { get; init; }
    public string? EmailAddress { get; init; }
    public string? ContactNumber { get; init; }
    public DateTimeOffset JoinAt { get; init; }
    public List<FollowerDto> Followers { get; init; } = [];
    public List<FollowingDto> Followings { get; init; } = [];
    public double FollowerCount => Followers.Count;
    public double FollowingCount => Followings.Count;
    public IReadOnlyCollection<UserPost> Posts { get; init; } = [];
    public int PostCount => Posts.Count;
    private class BaseMapping : Profile
    {
        public BaseMapping()
        {
            CreateMap<UserProfile, BaseUserDto>()
                .ForMember(dest => dest.UserId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.EmailAddress, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.ContactNumber, o => o.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.JoinAt, o => o.MapFrom(src => src.Created))
                .ForMember(dest => dest.Followers, o => o.MapFrom(src => src.Followed))
                .ForMember(dest => dest.Followings, o => o.MapFrom(src => src.Followers))
                .ForMember(dest => dest.Posts, o => o.MapFrom(src => src.Posts))
                .ForMember(dest => dest.PostCount, o => o.Ignore())
                ;

            CreateMap<Follows, FollowingDto>().ForMember(x => x.FollowedId, o => o.MapFrom(src => src.FollowedId));
            CreateMap<Follows, FollowerDto>().ForMember(x => x.FollowerId, o => o.MapFrom(src => src.FollowerId));

            CreateMap<Post, UserPost>()
                .ForMember(dest => dest.PostId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.PostOn, o => o.MapFrom(src => src.Created))
                .ForMember(dest => dest.Category, o => o.MapFrom(src => src.Category.ToString()));
        }
    }
}

public record UserDto : BaseUserDto
{
    public bool IsFollowed { get; set; }
    private class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserProfile, UserDto>()
                .IncludeBase<UserProfile, BaseUserDto>()
                .ForMember(dest => dest.IsFollowed, o => o.Ignore()); // We'll calculate it in the query handler.
        }
    }

}

public record FollowerDto
{
    public string? FollowerId { get; init; }
}

public record FollowingDto
{
    public string? FollowedId { get; init; }
}

public record UserPost
{
    public string? PostId { get; init; }
    public string? Content { get; init; }
    public DateTimeOffset PostOn { get; init; }
    public string? Category { get; init; }
}
