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
    protected AIAgent InnerAgent { get; }
    protected WorkspaceDefinition? Workspace { get; }
    public WorkspaceAIAgent(AIAgent innerAgent, WorkspaceDefinition? workspace)
    {
        InnerAgent = innerAgent;
        Workspace = workspace;
    }

    public override AgentThread DeserializeThread(JsonElement serializedThread, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return InnerAgent.DeserializeThread(serializedThread, jsonSerializerOptions);
    }

    public override AgentThread GetNewThread()
    {
        return InnerAgent.GetNewThread();
    }

    protected override Task<AgentRunResponse> RunCoreAsync(IEnumerable<ChatMessage> messages, AgentThread? thread = null, AgentRunOptions? options = null, CancellationToken cancellationToken = default)
    {
        return InnerAgent.RunAsync(GetChatMessages(messages), thread, options, cancellationToken);
    }

    protected override IAsyncEnumerable<AgentRunResponseUpdate> RunCoreStreamingAsync(IEnumerable<ChatMessage> messages, AgentThread? thread = null, AgentRunOptions? options = null, CancellationToken cancellationToken = default)
    {
        return InnerAgent.RunStreamingAsync(GetChatMessages(messages), thread, options, cancellationToken);
    }

    protected virtual IEnumerable<ChatMessage> GetChatMessages(IEnumerable<ChatMessage> messages)
    {
        var unionMessages = new List<ChatMessage>();

        if (Workspace != null)
        {
            if (!Workspace.SystemPrompt.IsNullOrWhiteSpace())
            {
                unionMessages.Add(new ChatMessage(ChatRole.System, Workspace.SystemPrompt));
            }
            if (!Workspace.Instructions.IsNullOrWhiteSpace())
            {
                unionMessages.Add(new ChatMessage(ChatRole.System, Workspace.Instructions));
            }
        }
        return unionMessages.Union(messages);
    }
}
