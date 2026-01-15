using LINGYUN.Abp.AI.Tools;
using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public class WorkspaceChatClient : IWorkspaceChatClient
{
    public IChatClient Client { get; }
    public WorkspaceDefinition? Workspace { get; }
    public WorkspaceChatClient(
        IChatClient chatClient,
        WorkspaceDefinition? workspace = null)
    {
        Workspace = workspace;
        Client = chatClient;
    }

    public virtual Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Client.GetResponseAsync(GetChatMessages(messages), GetChatOptions(options), cancellationToken);
    }

    public virtual IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Client.GetStreamingResponseAsync(GetChatMessages(messages), GetChatOptions(options), cancellationToken);
    }

    public virtual object? GetService(Type serviceType, object? serviceKey = null)
    {
        return Client.GetService(serviceType, serviceKey);
    }

    public virtual void Dispose()
    {
        Client.Dispose();
    }

    protected virtual ChatOptions GetChatOptions(ChatOptions? options)
    {
        options ??= new ChatOptions();
        options.Instructions = Workspace?.Instructions;
        options.Temperature ??= Workspace?.Temperature;
        options.MaxOutputTokens ??= Workspace?.MaxOutputTokens;
        options.FrequencyPenalty ??= Workspace?.FrequencyPenalty;
        options.PresencePenalty ??= Workspace?.PresencePenalty;

        return options;
    }

    protected virtual IEnumerable<ChatMessage> GetChatMessages(IEnumerable<ChatMessage> messages)
    {
        if (Workspace?.SystemPrompt?.IsNullOrWhiteSpace() == false &&
            !messages.Any(msg => msg.Role == ChatRole.System))
        {
            // 加入系统提示词
            messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, Workspace.SystemPrompt)
            }.Union(messages);
        }

        return messages;
    }
}
