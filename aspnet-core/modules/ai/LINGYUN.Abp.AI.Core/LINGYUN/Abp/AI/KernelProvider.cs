using LINGYUN.Abp.AI.Workspaces;
using Microsoft.SemanticKernel;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI;
public abstract class KernelProvider : IKernelProvider, ITransientDependency
{
    public abstract string Name { get; }

    protected IServiceProvider ServiceProvider { get; }
    protected KernelProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public abstract Task<Kernel> CreateAsync(WorkspaceDefinition workspace);
}
