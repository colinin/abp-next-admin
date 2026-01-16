using LINGYUN.Abp.AI.Chats;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Tokens;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;
using AIChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace LINGYUN.Abp.AI.Agent;
public class AgentService : IAgentService, IScopedDependency
{
    private readonly IClock _clock;
    private readonly IAgentFactory _agentFactory;
    private readonly ITokenUsageStore _tokenUsageStore;
    private readonly IChatMessageStore _chatMessageStore;
    public AgentService(
        IClock clock,
        IAgentFactory agentFactory, 
        ITokenUsageStore tokenUsageStore,
        IChatMessageStore chatMessageStore)
    {
        _clock = clock;
        _agentFactory = agentFactory;
        _tokenUsageStore = tokenUsageStore;
        _chatMessageStore = chatMessageStore;
    }

    public async virtual IAsyncEnumerable<string> SendMessageAsync(Models.ChatMessage message)
    {
        var messages = await BuildChatMessages(message);

        var agent = await _agentFactory.CreateAsync(message.Workspace);

        var agentRunRes = agent.RunStreamingAsync(messages);

        var tokenUsageInfo = new TokenUsageInfo(message.Workspace);
        var agentMessageBuilder = new StringBuilder();

        await foreach (var response in agentRunRes)
        {
            UpdateTokenUsageInfo(tokenUsageInfo, response);
            agentMessageBuilder.Append(response.Text);
            yield return response.Text;
        }

        var messageId = await StoreChatMessage(message, agentMessageBuilder.ToString());

        tokenUsageInfo.WithConversationId(message.ConversationId);
        tokenUsageInfo.WithMessageId(messageId);

        Console.WriteLine();
        Console.WriteLine($"消耗Token: {tokenUsageInfo}");

        await StoreTokenUsageInfo(tokenUsageInfo);
    }

    protected virtual async Task<IEnumerable<AIChatMessage>> BuildChatMessages(Models.ChatMessage message)
    {
        var messages = new List<AIChatMessage>();

        if (!message.ConversationId.IsNullOrWhiteSpace())
        {
            var historyMessages = await _chatMessageStore.GetHistoryMessagesAsync(message.ConversationId);

            foreach (var chatMessage in historyMessages)
            {
                messages.Add(new AIChatMessage(ChatRole.System, chatMessage.GetMessagePrompt()));
            }
        }

        messages.Add(new AIChatMessage(ChatRole.User, message.GetMessagePrompt()));

        return messages;
    }

    protected async virtual Task<string> StoreChatMessage(Models.ChatMessage message, string agentMessage)
    {
        message.WithReply(agentMessage, _clock.Now);

        return await _chatMessageStore.SaveMessageAsync(message);
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
