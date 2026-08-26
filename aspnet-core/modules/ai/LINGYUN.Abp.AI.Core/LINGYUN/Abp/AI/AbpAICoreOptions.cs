using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.AI;
public class AbpAICoreOptions
{
    public ITypeList<IWorkspaceDefinitionProvider> DefinitionProviders { get; }
    public ITypeList<IChatClientProvider> ChatClientProviders { get; }
    public ITypeList<IKernelProvider> KernelProviders { get; }

    public List<Action<WorkspaceDefinition, IKernelBuilder>> KernelBuildActions { get; }
    public List<Func<WorkspaceDefinition, IServiceProvider, ChatClientBuilder, Task<ChatClientBuilder>>> ChatClientBuildActions { get; }

    public HashSet<string> DeletedWorkspaces { get; }

    public AbpAICoreOptions()
    {
        DefinitionProviders = new TypeList<IWorkspaceDefinitionProvider>();
        ChatClientProviders = new TypeList<IChatClientProvider>();
        KernelProviders = new TypeList<IKernelProvider>();

        DeletedWorkspaces = new HashSet<string>();
        KernelBuildActions = new List<Action<WorkspaceDefinition, IKernelBuilder>>();
        ChatClientBuildActions = new List<Func<WorkspaceDefinition, IServiceProvider, ChatClientBuilder, Task<ChatClientBuilder>>>();
    }
}
