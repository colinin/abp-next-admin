using LINGYUN.Abp.AI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Messages;
public interface IUserMessageStore
{
    Task<string> SaveMessageAsync(UserMessage message);

    Task<IEnumerable<UserMessage>> GetHistoryMessagesAsync(string conversationId);
}
