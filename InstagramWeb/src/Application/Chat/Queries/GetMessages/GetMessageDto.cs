using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramWeb.Domain.Entities;
using InstagramWeb.Domain.Enums;

namespace InstagramWeb.Application.Chat.Queries.GetMessages;
public class GetMessageDto
{
    public MessageStatus Status { get; set; }
    public string Message { get; set; }=string.Empty;
    public DateTimeOffset Created { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public bool IsMine { get; set; }

}
