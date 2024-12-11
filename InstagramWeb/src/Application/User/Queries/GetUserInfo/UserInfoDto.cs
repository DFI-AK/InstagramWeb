using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.User.Queries.GetUserInfo;
public record UserInfoDto
{
}

//public record UserDto
//{
//    public string? UserId { get; init; }

//    public string? FullName { get; init; }

//    public string? EmailAddress { get; init; }

//    public string? ContactNumber { get; init; }

//    public DateTimeOffset JoinAt { get; init; }

//    public List<FollowerDto> Followers { get; init; } = [];

//    public List<FollowingDto> Followings { get; init; } = [];

//    public double FollowerCount => Followers.Count;

//    public double FollowingCount => Followings.Count;

//    private class Mapping : Profile
//    {
//        public Mapping()
//        {
//            CreateMap<UserProfile, UserDto>()
//                .ForMember(dest => dest.UserId, o => o.MapFrom(src => src.Id))
//                .ForMember(dest => dest.FullName, o => o.MapFrom(src => src.FirstName + ' ' + src.LastName))
//                .ForMember(dest => dest.EmailAddress, o => o.MapFrom(src => src.Email))
//                .ForMember(dest => dest.ContactNumber, o => o.MapFrom(src => src.PhoneNumber))
//                .ForMember(dest => dest.JoinAt, o => o.MapFrom(src => src.Created))
//                .ForMember(dest => dest.Followers, o => o.MapFrom(src => src.Followed))
//                .ForMember(dest => dest.Followings, o => o.MapFrom(src => src.Followers));

//            CreateMap<Follows, FollowingDto>().ForMember(x => x.FollowedId, o => o.MapFrom(src => src.FollowedId));
//            CreateMap<Follows, FollowerDto>().ForMember(x => x.FollowerId, o => o.MapFrom(src => src.FollowerId));

//        }
//    }
//}
