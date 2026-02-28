using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AI;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI;
public class ChatClientFactory : IChatClientFactory, ISingletonDependency
{
    private readonly static ConcurrentDictionary<string, IChatClient> _chatClientCache = new();
    protected IWorkspaceDefinitionManager WorkspaceDefinitionManager { get; }
    protected IChatClientProviderManager ChatClientProviderManager { get; }
    protected IServiceProvider ServiceProvider { get; }

    public ChatClientFactory(
        IWorkspaceDefinitionManager workspaceDefinitionManager,
        IChatClientProviderManager chatClientProviderManager,
        IServiceProvider serviceProvider)
    {
        WorkspaceDefinitionManager = workspaceDefinitionManager;
        ChatClientProviderManager = chatClientProviderManager;
        ServiceProvider = serviceProvider;
    }

    public async virtual Task<IChatClient> CreateAsync<TWorkspace>()
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
            chatClient = accessor.ChatClient;
            _chatClientCache.TryAdd(workspace, chatClient);
        }
        else
        {
            chatClient = await CreateAsync(workspace);
        }
        return chatClient;
    }

    public async virtual Task<IChatClient> CreateAsync(string workspace)
    {
        if (_chatClientCache.TryGetValue(workspace, out var chatClient))
        {
            return chatClient;
        }

        var workspaceDefine = await WorkspaceDefinitionManager.GetAsync(workspace);

        chatClient = await CreateChatClientAsync(workspaceDefine);

        _chatClientCache.TryAdd(workspace, chatClient);

        return chatClient;
    }

    protected async virtual Task<IChatClient> CreateChatClientAsync(WorkspaceDefinition workspace)
    {
        foreach (var provider in ChatClientProviderManager.Providers)
        {
            if (!string.Equals(provider.Name, workspace.Provider))
            {
                continue;
            }

            return await provider.CreateAsync(workspace);
        }

        throw new AbpException($"The ChatClient provider implementation named {workspace.Provider} was not found");
    }
}
