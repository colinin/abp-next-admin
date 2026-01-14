using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Volo.Abp.AI;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Agent;
public class ChatClientAgentFactory : IChatClientAgentFactory, ISingletonDependency
{
    private readonly static ConcurrentDictionary<string, ChatClientAgent> _chatClientAgentCache = new();

    private readonly IChatClientFactory _chatClientFactory;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly IWorkspaceDefinitionManager _workspaceDefinitionManager;
    public ChatClientAgentFactory(
        IChatClientFactory chatClientFactory,
        IStringLocalizerFactory stringLocalizerFactory,
        IWorkspaceDefinitionManager workspaceDefinitionManager)
    {
        _chatClientFactory = chatClientFactory;
        _stringLocalizerFactory = stringLocalizerFactory;
        _workspaceDefinitionManager = workspaceDefinitionManager;
    }

    public async virtual Task<ChatClientAgent> CreateAsync<TWorkspace>()
    {
        var workspace = WorkspaceNameAttribute.GetWorkspaceName<TWorkspace>();
        if (_chatClientAgentCache.TryGetValue(workspace, out var chatClientAgent))
        {
            return chatClientAgent;
        }

        var chatClient = await _chatClientFactory.CreateAsync<TWorkspace>();

        var workspaceDefine = await _workspaceDefinitionManager.GetOrNullAsync(workspace);

        string? description = null;
        if (workspaceDefine?.Description != null)
        {
            description = workspaceDefine.Description.Localize(_stringLocalizerFactory);
        }

        chatClientAgent = chatClient.CreateAIAgent(
            instructions: workspaceDefine?.SystemPrompt,
            name: workspaceDefine?.Name,
            description: description);

        _chatClientAgentCache.TryAdd(workspace, chatClientAgent);

        return chatClientAgent;
    }

    public async virtual Task<ChatClientAgent> CreateAsync(string workspace)
    {
        if (_chatClientAgentCache.TryGetValue(workspace, out var chatClientAgent))
        {
            return chatClientAgent;
        }
        var workspaceDefine = await _workspaceDefinitionManager.GetAsync(workspace);
        var chatClient = await _chatClientFactory.CreateAsync(workspace);

        string? description = null;
        if (workspaceDefine.Description != null)
        {
            description = workspaceDefine.Description.Localize(_stringLocalizerFactory);
        }

        chatClientAgent = chatClient.CreateAIAgent(
            instructions: workspaceDefine.SystemPrompt,
            name: workspaceDefine.Name,
            description: description);

        _chatClientAgentCache.TryAdd(workspace, chatClientAgent);

        return chatClientAgent;
    }
}
