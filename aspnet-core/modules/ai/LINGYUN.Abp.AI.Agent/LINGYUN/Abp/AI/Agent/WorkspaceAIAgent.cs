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

    protected override ValueTask<AgentSession> CreateSessionCoreAsync(CancellationToken cancellationToken = default)
    {
        return InnerAgent.CreateSessionAsync(cancellationToken);
    }

    protected override ValueTask<JsonElement> SerializeSessionCoreAsync(AgentSession session, JsonSerializerOptions? jsonSerializerOptions = null, CancellationToken cancellationToken = default)
    {
        return InnerAgent.SerializeSessionAsync(session, jsonSerializerOptions, cancellationToken);
    }

    protected override ValueTask<AgentSession> DeserializeSessionCoreAsync(JsonElement serializedState, JsonSerializerOptions? jsonSerializerOptions = null, CancellationToken cancellationToken = default)
    {
        return InnerAgent.DeserializeSessionAsync(serializedState, jsonSerializerOptions, cancellationToken);
    }

    protected override Task<AgentResponse> RunCoreAsync(IEnumerable<ChatMessage> messages, AgentSession? session = null, AgentRunOptions? options = null, CancellationToken cancellationToken = default)
    {
        return InnerAgent.RunAsync(GetChatMessages(messages), session, options, cancellationToken);
    }

    protected override IAsyncEnumerable<AgentResponseUpdate> RunCoreStreamingAsync(IEnumerable<ChatMessage> messages, AgentSession? session = null, AgentRunOptions? options = null, CancellationToken cancellationToken = default)
    {
        return InnerAgent.RunStreamingAsync(GetChatMessages(messages), session, options, cancellationToken);
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
