using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.Common.Models;
public record ChatWindowDto
{
    public string? MessageId { get; set; }
    public List<MessageDto> Messages { get; set; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserMessage, MessageDto>()
           .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.TextMessage, opt => opt.MapFrom(src => src.TextMessage))
           .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.Created))
           .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => src.Status))
           .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
           .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver));
        }
    }
}
