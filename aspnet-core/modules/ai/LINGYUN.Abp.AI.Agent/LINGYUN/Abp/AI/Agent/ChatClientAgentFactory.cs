using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Volo.Abp.AI;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI.Agent;
public class ChatClientAgentFactory : IChatClientAgentFactory, ISingletonDependency
{
    private readonly static ConcurrentDictionary<string, ChatClientAgent> _chatClientAgentCache = new();

    protected IChatClientFactory ChatClientFactory { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected IWorkspaceDefinitionManager WorkspaceDefinitionManager { get; }
    protected ISimpleStateCheckerManager<WorkspaceDefinition> StateCheckerManager { get; }
    public ChatClientAgentFactory(
        IChatClientFactory chatClientFactory,
        IStringLocalizerFactory stringLocalizerFactory,
        IWorkspaceDefinitionManager workspaceDefinitionManager,
        ISimpleStateCheckerManager<WorkspaceDefinition> stateCheckerManager)
    {
        ChatClientFactory = chatClientFactory;
        StringLocalizerFactory = stringLocalizerFactory;
        WorkspaceDefinitionManager = workspaceDefinitionManager;
        StateCheckerManager = stateCheckerManager;
    }

    public async virtual Task<ChatClientAgent> CreateAsync<TWorkspace>()
    {
        var workspace = WorkspaceNameAttribute.GetWorkspaceName<TWorkspace>();
        if (_chatClientAgentCache.TryGetValue(workspace, out var chatClientAgent))
        {
            return chatClientAgent;
        }

        var chatClient = await ChatClientFactory.CreateAsync<TWorkspace>();

        var workspaceDefine = await WorkspaceDefinitionManager.GetOrNullAsync(workspace);

        if (workspaceDefine != null)
        {
            await CheckWorkspaceStateAsync(workspaceDefine);
        }

        string? description = null;
        if (workspaceDefine?.Description != null)
        {
            description = workspaceDefine.Description.Localize(StringLocalizerFactory);
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
        var workspaceDefine = await WorkspaceDefinitionManager.GetAsync(workspace);

        await CheckWorkspaceStateAsync(workspaceDefine);

        var chatClient = await ChatClientFactory.CreateAsync(workspace);

        string? description = null;
        if (workspaceDefine.Description != null)
        {
            description = workspaceDefine.Description.Localize(StringLocalizerFactory);
        }

        chatClientAgent = chatClient.CreateAIAgent(
            instructions: workspaceDefine.SystemPrompt,
            name: workspaceDefine.Name,
            description: description);

        _chatClientAgentCache.TryAdd(workspace, chatClientAgent);

        return chatClientAgent;
    }

    protected async virtual Task CheckWorkspaceStateAsync(WorkspaceDefinition workspace)
    {
        if (!await StateCheckerManager.IsEnabledAsync(workspace))
        {
            throw new AbpAuthorizationException(
                $"Workspace is not enabled: {workspace.Name}!",
                AbpAIErrorCodes.WorkspaceIsNotEnabled)
                .WithData("Workspace", workspace.Name);
        }
    }
}
