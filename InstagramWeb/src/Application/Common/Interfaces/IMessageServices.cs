using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramWeb.Application.Chat.Queries.GetMessages;

namespace InstagramWeb.Application.Common.Interfaces;
public interface IMessageServices
{
    public Task<List<GetMessageDto>> GetAllMessages(string UserId);
}
