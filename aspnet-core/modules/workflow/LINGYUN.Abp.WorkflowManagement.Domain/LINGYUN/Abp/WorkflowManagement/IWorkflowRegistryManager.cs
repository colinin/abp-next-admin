using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement
{
    public interface IWorkflowRegistryManager
    {
        Task RegisterAsync(CancellationToken cancellationToken = default);
    }
}
