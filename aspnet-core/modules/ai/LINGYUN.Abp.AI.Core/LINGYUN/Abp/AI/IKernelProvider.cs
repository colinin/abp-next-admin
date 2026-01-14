using LINGYUN.Abp.AI.Workspaces;
using Microsoft.SemanticKernel;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IKernelProvider
{
    string Name { get; }

    Task<Kernel> CreateAsync(WorkspaceDefinition workspace);
}
