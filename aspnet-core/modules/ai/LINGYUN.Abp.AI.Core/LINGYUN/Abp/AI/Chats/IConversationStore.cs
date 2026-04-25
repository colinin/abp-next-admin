using LINGYUN.Abp.AI.Models;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Chats;
public interface IConversationStore
{
    Task SaveAsync(Conversation conversation);

    Task<Conversation?> FindAsync(Guid conversationId);

    Task CleanupAsync();
}
