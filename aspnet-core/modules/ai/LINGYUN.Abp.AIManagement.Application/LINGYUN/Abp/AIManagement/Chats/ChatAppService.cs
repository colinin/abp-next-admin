using LINGYUN.Abp.AI.Agent;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Chats.Dtos;
using Microsoft.Extensions.AI;
using System.Collections.Generic;

namespace LINGYUN.Abp.AIManagement.Chats;
public class ChatAppService : AIManagementApplicationServiceBase, IChatAppService
{
    private readonly IAgentService _agentService;
    public ChatAppService(IAgentService agentService)
    {
        _agentService = agentService;
    }

    public IAsyncEnumerable<string> SendMessageAsync(SendTextChatMessageDto input)
    {
        var chatMessage = new TextChatMessage(
            input.Workspace,
            input.Content,
            ChatRole.User,
            Clock.Now);

        chatMessage.WithConversationId(input.ConversationId);

        return _agentService.SendMessageAsync(chatMessage);
    }
}
