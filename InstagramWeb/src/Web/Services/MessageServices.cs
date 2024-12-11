using InstagramWeb.Application.Chat.Queries.GetMessages;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Web.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace InstagramWeb.Web.Services;

public class MessageServices(IUser user, IApplicationDbContext context) : IMessageServices
{
    public IUser _user = user;
    public IApplicationDbContext _context = context;
    public async Task<List<GetMessageDto>> GetAllMessages(string userId)
    {
        if (_user == null)
        {
            return [];
        }

        string loginUserId = _user.Id ?? "";
        List<GetMessageDto> allMessages = await _context.Messages
     .Where(m => (m.SenderId == loginUserId && m.ReceiverId == userId) ||
                 (m.SenderId == userId && m.ReceiverId == loginUserId))
     .OrderBy(m => m.Created) 
     .Select(x => new GetMessageDto
     {
         Created = x.Created,
         IsMine = x.SenderId == loginUserId,
         Message = x.TextMessage ?? "",
         Status = x.Status,
         SenderName = x.Sender.FirstName ?? ""
     })
     .ToListAsync();

        return allMessages;
    }
}
