using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WorkflowManagement
{
    public class WorkflowRegistryManager : IWorkflowRegistryManager, ITransientDependency
    {
        private readonly WorkflowManager _workflowManager;

        private readonly IWorkflowRepository _workflowRepository;
        private readonly IStepNodeRepository _stepNodeRepository;
        private readonly ICompensateNodeRepository _compensateNodeRepository;

        public WorkflowRegistryManager(
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

        public virtual async Task RegisterAsync(CancellationToken cancellationToken = default)
        {
            // TODO: 多租户如何注册?

            var workflows = await _workflowRepository
                .GetListAsync(x => x.IsEnabled, cancellationToken: cancellationToken);

            foreach (var workflow in workflows)
            {
                if (!_workflowManager.IsRegistered(workflow))
                {
                    var stepNodes = await _stepNodeRepository
                        .GetAllChildrenWithWorkflowAsync(workflow.Id, cancellationToken: cancellationToken);
                    var compensateNodes = await _compensateNodeRepository
                        .GetAllChildrenWithWorkflowAsync(workflow.Id, cancellationToken: cancellationToken);

                    _workflowManager.Register(workflow, stepNodes, compensateNodes);
                }
            }
        }
    }
}
