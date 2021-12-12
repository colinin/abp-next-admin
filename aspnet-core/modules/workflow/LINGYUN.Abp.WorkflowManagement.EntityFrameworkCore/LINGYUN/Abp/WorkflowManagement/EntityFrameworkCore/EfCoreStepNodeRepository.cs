using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    public class EfCoreStepNodeRepository : EfCoreRepository<WorkflowManagementDbContext, StepNode, Guid>, IStepNodeRepository
    {
        public EfCoreStepNodeRepository(
            IDbContextProvider<WorkflowManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<StepNode>> GetAllChildrenWithWorkflowAsync(Guid workflowId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.WorkflowId.Equals(workflowId))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
