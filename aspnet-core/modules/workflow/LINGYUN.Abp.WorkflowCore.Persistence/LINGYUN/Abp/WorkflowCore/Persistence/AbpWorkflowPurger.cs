using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class AbpWorkflowPurger : IWorkflowPurger
    {
        private readonly IWorkflowRepository _workflowRepository;

        public AbpWorkflowPurger(
            IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public virtual async Task PurgeWorkflows(WorkflowStatus status, DateTime olderThan)
        {
            var olderThanUtc = olderThan.ToUniversalTime();

            var workflows = await _workflowRepository
                .GetOlderListAsync(status, olderThanUtc, includeDetails: true);

            await _workflowRepository.DeleteManyAsync(workflows);
        }
    }
}
