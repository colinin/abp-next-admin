using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowManagement
{
    public class WorkflowRegisterService : BackgroundService
    {
        private readonly WorkflowManager _workflowManager;

        private readonly IWorkflowRepository _workflowRepository;
        private readonly IStepNodeRepository _stepNodeRepository;
        private readonly ICompensateNodeRepository _compensateNodeRepository;

        public WorkflowRegisterService(
            WorkflowManager workflowManager,
            IWorkflowRepository workflowRepository,
            IStepNodeRepository stepNodeRepository,
            ICompensateNodeRepository compensateNodeRepository)
        {
            _workflowManager = workflowManager;
            _workflowRepository = workflowRepository;
            _stepNodeRepository = stepNodeRepository;
            _compensateNodeRepository = compensateNodeRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workflows = await _workflowRepository.GetListAsync(cancellationToken: stoppingToken);

            foreach (var workflow in workflows)
            {
                var stepNodes = await _stepNodeRepository.GetAllChildrenWithWorkflowAsync(workflow.Id, cancellationToken: stoppingToken);
                var compensateNodes = await _compensateNodeRepository.GetAllChildrenWithWorkflowAsync(workflow.Id, cancellationToken: stoppingToken);

                _workflowManager.Register(workflow, stepNodes, compensateNodes);
            }
        }
    }
}
