using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowManagement
{
    public interface ICompensateNodeRepository : IRepository<CompensateNode, Guid>
    {
        Task<List<CompensateNode>> GetAllChildrenWithWorkflowAsync(
            Guid workflowId,
            CancellationToken cancellationToken = default);
    }
}
