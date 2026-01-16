using LINGYUN.Abp.AI.Models;
using System.Collections.Generic;

namespace LINGYUN.Abp.AI.Agent;
public interface IAgentService
{
    IAsyncEnumerable<string> SendMessageAsync(ChatMessage message);
}
