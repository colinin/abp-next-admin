using JetBrains.Annotations;
using Microsoft.SemanticKernel;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IKernelFactory
{
    [NotNull]
    Task<Kernel> CreateAsync<TWorkspace>();

    [NotNull]
    Task<Kernel> CreateAsync(string workspace);
}
