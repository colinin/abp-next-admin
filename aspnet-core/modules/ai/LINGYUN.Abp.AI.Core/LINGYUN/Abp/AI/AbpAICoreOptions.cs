using LINGYUN.Abp.AI.Workspaces;
using System.Collections.Generic;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.AI;
public class AbpAICoreOptions
{
    public ITypeList<IWorkspaceDefinitionProvider> DefinitionProviders { get; }
    public ITypeList<IChatClientProvider> ChatClientProviders { get; }
    public ITypeList<IKernelProvider> KernelProviders { get; }

    public HashSet<string> DeletedWorkspaces { get; }

    public AbpAICoreOptions()
    {
        DefinitionProviders = new TypeList<IWorkspaceDefinitionProvider>();
        ChatClientProviders = new TypeList<IChatClientProvider>();
        KernelProviders = new TypeList<IKernelProvider>();

        DeletedWorkspaces = new HashSet<string>();
    }
}
