using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement
{
    public interface IWorkflowEngineManager
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);

        Task StartAsync(CancellationToken cancellationToken = default);
    }
}
