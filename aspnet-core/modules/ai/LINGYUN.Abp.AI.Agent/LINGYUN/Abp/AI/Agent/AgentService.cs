using LINGYUN.Abp.AI.Chats;
using LINGYUN.Abp.AI.Localization;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Tokens;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using AIChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace LINGYUN.Abp.AI.Agent;
public class AgentService : IAgentService, IScopedDependency
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IAgentFactory _agentFactory;
    private readonly ITokenUsageStore _tokenUsageStore;
    private readonly IChatMessageStore _chatMessageStore;
    private readonly IConversationStore _conversationStore;
    private readonly IStringLocalizer<AbpAIResource> _localizerResource;
    public AgentService(
        IClock clock,
        IGuidGenerator guidGenerator,
        IAgentFactory agentFactory, 
        ITokenUsageStore tokenUsageStore,
        IChatMessageStore chatMessageStore,
        IConversationStore conversationStore,
        IStringLocalizer<AbpAIResource> localizerResource)
    {
        _clock = clock;
        _guidGenerator = guidGenerator;
        _agentFactory = agentFactory;
        _tokenUsageStore = tokenUsageStore;
        _chatMessageStore = chatMessageStore;
        _conversationStore = conversationStore;
        _localizerResource = localizerResource;
    }

    public async virtual IAsyncEnumerable<string> SendMessageAsync(Models.ChatMessage message)
    {
        var conversationId = await StoreConversation(message);

        message.WithConversationId(conversationId);

        var messages = await BuildChatMessages(message);

        var agent = await _agentFactory.CreateAsync(message.Workspace);

        var agentRunRes = agent.RunStreamingAsync(messages);

        var tokenUsageInfo = new TokenUsageInfo(message.Workspace, conversationId);
        var agentMessageBuilder = new StringBuilder();

        await foreach (var response in agentRunRes)
        {
            UpdateTokenUsageInfo(tokenUsageInfo, response);
            agentMessageBuilder.Append(response.Text);
            yield return response.Text;
        }

        var messageId = await StoreChatMessage(message, agentMessageBuilder.ToString());

        tokenUsageInfo.WithMessageId(messageId);

#if DEBUG
        Console.WriteLine();
        Console.WriteLine(tokenUsageInfo);
#endif

        await StoreTokenUsageInfo(tokenUsageInfo);
    }

    protected virtual async Task<IEnumerable<AIChatMessage>> BuildChatMessages(Models.ChatMessage message)
    {
        var messages = new List<AIChatMessage>();

        if (message.ConversationId.HasValue)
        {
            var historyMessages = await _chatMessageStore.GetHistoryMessagesAsync(message.ConversationId.Value);

            // TODO: 应用摘要提示压缩
            foreach (var chatMessage in historyMessages)
            {
                messages.Add(new AIChatMessage(chatMessage.Role, chatMessage.GetMessagePrompt()));
            }
        }

        messages.Add(new AIChatMessage(ChatRole.User, message.GetMessagePrompt()));

        return messages;
    }

    protected async virtual Task<Guid> StoreChatMessage(Models.ChatMessage message, string agentMessage)
    {
        message.WithReply(agentMessage, _clock.Now);

        return await _chatMessageStore.SaveMessageAsync(message);
    }

    protected async virtual Task<Guid> StoreConversation(Models.ChatMessage message)
    {
        if (message.ConversationId.HasValue)
        {
            var conversation = await _conversationStore.FindAsync(message.ConversationId.Value);
            if (conversation == null || conversation.ExpiredAt <= _clock.Now)
            {
                throw new BusinessException(
                    AbpAIErrorCodes.ConversationHasExpired,
                    "The conversation has expired. Please create a new one!");
            }

            conversation.UpdateAt = _clock.Now;
            await _conversationStore.SaveAsync(conversation);

            return conversation.Id;
        }
        else
        {
            var conversation = new Conversation(
                _guidGenerator.Create(),
                _localizerResource["NewConversation"],
                _clock.Now);

            await _conversationStore.SaveAsync(conversation);

            return conversation.Id;
        }
    }

    protected async virtual Task StoreTokenUsageInfo(TokenUsageInfo tokenUsageInfo)
    {
        await _tokenUsageStore.SaveTokenUsageAsync(tokenUsageInfo);
    }

    private static void UpdateTokenUsageInfo(TokenUsageInfo tokenUsageInfo, AgentRunResponseUpdate response)
    {
        if (response.RawRepresentation != null &&
            response.RawRepresentation is ChatResponseUpdate update)
        {
            var usageContents = update.Contents.OfType<UsageContent>();

            tokenUsageInfo.InputTokenCount = usageContents.Max(x => x.Details.InputTokenCount);
            tokenUsageInfo.OutputTokenCount = usageContents.Max(x => x.Details.OutputTokenCount);
            tokenUsageInfo.TotalTokenCount = usageContents.Max(x => x.Details.TotalTokenCount);
            tokenUsageInfo.ReasoningTokenCount = usageContents.Max(x => x.Details.ReasoningTokenCount);
            tokenUsageInfo.CachedInputTokenCount = usageContents.Max(x => x.Details.CachedInputTokenCount);
        }
    }
}
