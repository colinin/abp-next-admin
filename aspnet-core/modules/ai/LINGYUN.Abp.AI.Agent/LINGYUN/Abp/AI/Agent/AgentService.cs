using LINGYUN.Abp.AI.Messages;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Tokens;
using Microsoft.Extensions.AI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Agent;
public class AgentService : IAgentService, IScopedDependency
{
    private readonly IAgentFactory _agentFactory;
    private readonly ITokenUsageStore _tokenUsageStore;
    private readonly IUserMessageStore _userMessageStore;
    public AgentService(
        IAgentFactory agentFactory, 
        ITokenUsageStore tokenUsageStore,
        IUserMessageStore userMessageStore)
    {
        _agentFactory = agentFactory;
        _tokenUsageStore = tokenUsageStore;
        _userMessageStore = userMessageStore;
    }

    public async virtual IAsyncEnumerable<string> SendMessageAsync(UserMessage message)
    {
        var messages = await BuildChatMessages(message);

        var agent = await _agentFactory.CreateAsync(message.Workspace);

        var agentRunRes = agent.RunStreamingAsync(messages);

        var agentMessageBuilder = new StringBuilder();

        await foreach (var item in agentRunRes)
        {
            agentMessageBuilder.Append(item);
            yield return item.Text;

            await StoreTokenUsageInfo(message, item.RawRepresentation);
        }

        await StoreChatMessage(message, agentMessageBuilder.ToString());
    }

    protected virtual async Task<IEnumerable<ChatMessage>> BuildChatMessages(UserMessage message)
    {
        var messages = new List<ChatMessage>();

        var historyMessages = await _userMessageStore.GetHistoryMessagesAsync(message.ChatId);

        foreach (var chatMessage in historyMessages)
        {
            messages.Add(new ChatMessage(ChatRole.System, chatMessage.Content));
        }

        messages.Add(new ChatMessage(ChatRole.User, message.Content));

        return messages;
    }

    protected async virtual Task StoreChatMessage(UserMessage message, string agentMessage)
    {
        message.WithReply(agentMessage);

        await _userMessageStore.SaveMessageAsync(message);
    }

    protected async virtual Task StoreTokenUsageInfo(UserMessage message, object? rawRepresentation)
    {
        if (rawRepresentation is ChatResponseUpdate update)
        {
            var tokenUsageInfos = update.Contents
                .OfType<UsageContent>()
                .Where(usage => usage.Details != null)
                .Select(usage => new TokenUsageInfo(message.Workspace)
                {
                    TotalTokenCount = usage.Details.TotalTokenCount,
                    CachedInputTokenCount = usage.Details.CachedInputTokenCount,
                    InputTokenCount = usage.Details.InputTokenCount,
                    AdditionalCounts = usage.Details.AdditionalCounts,
                    OutputTokenCount = usage.Details.OutputTokenCount,
                    ReasoningTokenCount = usage.Details.ReasoningTokenCount,
                });

            await _tokenUsageStore.SaveTokenUsagesAsync(tokenUsageInfos);
        }
    }
}
