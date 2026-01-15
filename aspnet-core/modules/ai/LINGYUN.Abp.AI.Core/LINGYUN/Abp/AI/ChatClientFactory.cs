using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AI;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI;
public class ChatClientFactory : IChatClientFactory, ISingletonDependency
{
    private readonly static ConcurrentDictionary<string, IWorkspaceChatClient> _chatClientCache = new();
    protected ISimpleStateCheckerManager<WorkspaceDefinition> StateCheckerManager { get; }
    protected IWorkspaceDefinitionManager WorkspaceDefinitionManager { get; }
    protected IChatClientProviderManager ChatClientProviderManager { get; }
    protected IServiceProvider ServiceProvider { get; }

    public ChatClientFactory(
        ISimpleStateCheckerManager<WorkspaceDefinition> stateCheckerManager,
        IWorkspaceDefinitionManager workspaceDefinitionManager,
        IChatClientProviderManager chatClientProviderManager,
        IServiceProvider serviceProvider)
    {
        StateCheckerManager = stateCheckerManager;
        WorkspaceDefinitionManager = workspaceDefinitionManager;
        ChatClientProviderManager = chatClientProviderManager;
        ServiceProvider = serviceProvider;
    }

    public async virtual Task<IWorkspaceChatClient> CreateAsync<TWorkspace>()
    {
        var workspace = WorkspaceNameAttribute.GetWorkspaceName<TWorkspace>();
        if (_chatClientCache.TryGetValue(workspace, out var chatClient))
        {
            return chatClient;
        }

        var chatClientAccessorType = typeof(IChatClientAccessor<>).MakeGenericType(typeof(TWorkspace));
        var chatClientAccessor = ServiceProvider.GetService(chatClientAccessorType);
        if (chatClientAccessor != null && 
            chatClientAccessor is IChatClientAccessor accessor && 
            accessor.ChatClient != null)
        {
            chatClient = new WorkspaceChatClient(accessor.ChatClient);
            _chatClientCache.TryAdd(workspace, chatClient);
        }
        else
        {
            chatClient = await CreateAsync(workspace);
        }
        return chatClient;
    }

    public async virtual Task<IWorkspaceChatClient> CreateAsync(string workspace)
    {
        if (_chatClientCache.TryGetValue(workspace, out var chatClient))
        {
            return chatClient;
        }

        var workspaceDefine = await WorkspaceDefinitionManager.GetAsync(workspace);

        await CheckWorkspaceStateAsync(workspaceDefine);

        chatClient = await CreateChatClientAsync(workspaceDefine);

        _chatClientCache.TryAdd(workspace, chatClient);

        return chatClient;
    }

    protected async virtual Task<IWorkspaceChatClient> CreateChatClientAsync(WorkspaceDefinition workspace)
    {
        foreach (var provider in ChatClientProviderManager.Providers)
        {
            if (!string.Equals(provider.Name, workspace.Provider))
            {
                continue;
            }

            return await provider.CreateAsync(workspace);
        }

        throw new AbpException($"The ChatClient provider implementation named {workspace.Provider} was not found!");
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
