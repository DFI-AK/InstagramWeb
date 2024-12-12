using InstagramWeb.Application.User.Queries.GetUsers;
using InstagramWeb.Domain.Entities;
using InstagramWeb.Domain.Enums;

namespace InstagramWeb.Application.Common.Models;
public record BaseMessageDto
{
    public string? MessageId { get; init; }
    public string? TextMessage { get; init; }
    public MessageStatus MessageStatus { get; init; }
    public DateTimeOffset SentAt { get; init; }
    public UserDto Sender { get; set; } = null!;
    public UserDto Receiver { get; set; } = null!;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserMessage, BaseMessageDto>()
           .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.TextMessage, opt => opt.MapFrom(src => src.TextMessage))
           .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.Created))
           .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => src.Status))
           .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
           .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver));
        }
    }
}

public record MessageDto : BaseMessageDto
{
    public bool IsMine { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserMessage, MessageDto>()
                .IncludeBase<UserMessage, BaseMessageDto>()
                .ForMember(dest => dest.IsMine, o => o.Ignore());
        }
    }
}
