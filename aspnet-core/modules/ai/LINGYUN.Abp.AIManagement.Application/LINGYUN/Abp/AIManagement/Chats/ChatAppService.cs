using LINGYUN.Abp.AI.Agent;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Chats.Dtos;
using LINGYUN.Abp.AIManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.AI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.AIManagement.Chats;

[Authorize(AIManagementPermissionNames.Chat.Default)]
public class ChatAppService : AIManagementApplicationServiceBase, IChatAppService
{
    private readonly IAgentService _agentService;
    private readonly ITextChatMessageRecordRepository _repository;
    public ChatAppService(
        IAgentService agentService, 
        ITextChatMessageRecordRepository repository)
    {
        _agentService = agentService;
        _repository = repository;
    }

    [Authorize(AIManagementPermissionNames.Chat.SendMessage)]
    public virtual IAsyncEnumerable<string> SendMessageAsync(SendTextChatMessageDto input)
    {
        var chatMessage = new TextChatMessage(
            input.Workspace,
            input.Content,
            ChatRole.User,
            Clock.Now);

        chatMessage.WithConversationId(input.ConversationId);

        return _agentService.SendMessageAsync(chatMessage);
    }

    public async virtual Task<PagedResultDto<TextChatMessageDto>> GetListAsync(TextChatMessageGetListInput input)
    {
        var specification = new ExpressionSpecification<TextChatMessageRecord>(
            x => x.ConversationId == input.ConversationId);

        var totalCount = await _repository.GetCountAsync(specification);
        var textChatMessages = await _repository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<TextChatMessageDto>(totalCount,
            ObjectMapper.Map<List<TextChatMessageRecord>, List<TextChatMessageDto>>(textChatMessages));
    }
}
