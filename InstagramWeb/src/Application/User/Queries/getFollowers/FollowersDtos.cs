using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.User.Queries.getFollowers;
public record FollowersDtos
{
    public string userId {  get; init; } =string.Empty;

    public string FullName {  get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserProfile, FollowersDtos>()
                .ForMember(dest=>dest.userId,o=>o.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.EmailAddress,o=>o.MapFrom(src=>src.Email))
                .ForMember(dest=>dest.FullName,o=>o.MapFrom(src=>src.FirstName+' '+src.LastName));
        }
    }

}
