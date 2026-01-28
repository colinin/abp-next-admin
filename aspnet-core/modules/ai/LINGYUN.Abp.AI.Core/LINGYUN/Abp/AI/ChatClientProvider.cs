using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI;
public abstract class ChatClientProvider : IChatClientProvider, ITransientDependency
{
    public abstract string Name { get; }

    protected IServiceProvider ServiceProvider { get; }
    protected ChatClientProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public abstract Task<IChatClient> CreateAsync(WorkspaceDefinition workspace);
}
