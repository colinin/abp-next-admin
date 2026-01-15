using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AI;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI;
public class ChatClientFactory : IChatClientFactory, IScopedDependency
{
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

    public async virtual Task<IChatClient> CreateAsync<TWorkspace>()
    {
        var workspace = WorkspaceNameAttribute.GetWorkspaceName<TWorkspace>();

        var chatClientAccessorType = typeof(IChatClientAccessor<>).MakeGenericType(typeof(TWorkspace));
        var chatClientAccessor = ServiceProvider.GetService(chatClientAccessorType);
        if (chatClientAccessor != null && 
            chatClientAccessor is IChatClientAccessor accessor && 
            accessor.ChatClient != null)
        {
            return accessor.ChatClient;
        }

        return await CreateAsync(workspace);
    }

    public async virtual Task<IChatClient> CreateAsync(string workspace)
    {
        var workspaceDefine = await WorkspaceDefinitionManager.GetAsync(workspace);

        await CheckWorkspaceStateAsync(workspaceDefine);

        return await CreateChatClientAsync(workspaceDefine);
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
