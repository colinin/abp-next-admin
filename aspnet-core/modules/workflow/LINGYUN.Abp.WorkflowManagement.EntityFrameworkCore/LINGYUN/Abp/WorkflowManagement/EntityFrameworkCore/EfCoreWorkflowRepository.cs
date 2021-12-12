using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    public class EfCoreWorkflowRepository : EfCoreRepository<WorkflowManagementDbContext, Workflow, Guid>, IWorkflowRepository
    {
        public EfCoreWorkflowRepository(
            IDbContextProvider<WorkflowManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> CheckVersionAsync(string name, int version, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .AnyAsync(x => x.Name.Equals(name) && x.Version == version,
                    GetCancellationToken(cancellationToken));
        }
    }
}
