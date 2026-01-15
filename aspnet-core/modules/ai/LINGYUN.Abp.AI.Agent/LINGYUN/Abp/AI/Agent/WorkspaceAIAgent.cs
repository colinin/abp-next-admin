using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Agent;
public class WorkspaceAIAgent : AIAgent
{
    protected AIAgent InnerAIAgent { get; }
    public WorkspaceDefinition? Workspace { get; }

    public WorkspaceAIAgent(
        AIAgent innerAIAgent, 
        WorkspaceDefinition? workspace = null)
    {
        InnerAIAgent = innerAIAgent;
        Workspace = workspace;
    }

    public override AgentThread DeserializeThread(JsonElement serializedThread, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return InnerAIAgent.DeserializeThread(serializedThread, jsonSerializerOptions);
    }

    public override AgentThread GetNewThread()
    {
        return InnerAIAgent.GetNewThread();
    }

    protected override Task<AgentRunResponse> RunCoreAsync(IEnumerable<ChatMessage> messages, AgentThread? thread = null, AgentRunOptions? options = null, CancellationToken cancellationToken = default)
    {
        return InnerAIAgent.RunAsync(GetChatMessages(messages), thread, options, cancellationToken);
    }

    protected override IAsyncEnumerable<AgentRunResponseUpdate> RunCoreStreamingAsync(IEnumerable<ChatMessage> messages, AgentThread? thread = null, AgentRunOptions? options = null, CancellationToken cancellationToken = default)
    {
        return InnerAIAgent.RunStreamingAsync(GetChatMessages(messages), thread, options, cancellationToken);
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
