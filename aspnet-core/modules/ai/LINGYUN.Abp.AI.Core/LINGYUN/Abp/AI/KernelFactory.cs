using LINGYUN.Abp.AI.Workspaces;
using Microsoft.SemanticKernel;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AI;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI;
public class KernelFactory : IKernelFactory, IScopedDependency
{
    protected ISimpleStateCheckerManager<WorkspaceDefinition> StateCheckerManager { get; }
    protected IWorkspaceDefinitionManager WorkspaceDefinitionManager { get; }
    protected IKernelProviderManager KernelProviderManager { get; }
    protected IServiceProvider ServiceProvider { get; }

    public KernelFactory(
        ISimpleStateCheckerManager<WorkspaceDefinition> stateCheckerManager,
        IWorkspaceDefinitionManager workspaceDefinitionManager,
        IKernelProviderManager kernelProviderManager,
        IServiceProvider serviceProvider)
    {
        StateCheckerManager = stateCheckerManager;
        WorkspaceDefinitionManager = workspaceDefinitionManager;
        KernelProviderManager = kernelProviderManager;
        ServiceProvider = serviceProvider;
    }

    public async virtual Task<Kernel> CreateAsync<TWorkspace>()
    {
        var workspace = WorkspaceNameAttribute.GetWorkspaceName<TWorkspace>();

        var kernelAccessorType = typeof(IKernelAccessor<>).MakeGenericType(typeof(TWorkspace));
        var kernelAccessor = ServiceProvider.GetService(kernelAccessorType);
        if (kernelAccessor != null && 
            kernelAccessor is IKernelAccessor accessor 
            && accessor.Kernel != null)
        {
            return accessor.Kernel;
        }
        return await CreateAsync(workspace);
    }

    public async virtual Task<Kernel> CreateAsync(string workspace)
    {
        var workspaceDefine = await WorkspaceDefinitionManager.GetAsync(workspace);

        await CheckWorkspaceStateAsync(workspaceDefine);

        return await CreateKernelAsync(workspaceDefine);
    }

    protected async virtual Task<Kernel> CreateKernelAsync(WorkspaceDefinition workspace)
    {
        foreach (var provider in KernelProviderManager.Providers)
        {
            if (!string.Equals(provider.Name, workspace.Provider))
            {
                continue;
            }

            return await provider.CreateAsync(workspace);
        }

        throw new AbpException($"The Kernel provider implementation named {workspace.Provider} was not found!");
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
