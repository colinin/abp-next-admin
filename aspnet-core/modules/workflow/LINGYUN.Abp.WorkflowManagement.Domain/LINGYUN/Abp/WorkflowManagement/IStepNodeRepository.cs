using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowManagement
{
    public interface IStepNodeRepository : IRepository<StepNode, Guid>
    {
        Task<List<StepNode>> GetAllChildrenWithWorkflowAsync(
            Guid workflowId,
            CancellationToken cancellationToken = default);
    }
}
