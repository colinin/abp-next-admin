using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement
{
    public class WorkflowRegisterService : BackgroundService
    {
        private readonly IWorkflowRegistryManager _registryManager;

        public WorkflowRegisterService(
            IWorkflowRegistryManager registryManager)
        {
            _registryManager = registryManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _registryManager.RegisterAsync(stoppingToken);
        }
    }
}
