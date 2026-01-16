using LINGYUN.Abp.AI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Chats;
public interface IChatMessageStore
{
    Task<string> SaveMessageAsync(ChatMessage message);

    Task<IEnumerable<ChatMessage>> GetHistoryMessagesAsync(Guid conversationId);
}
