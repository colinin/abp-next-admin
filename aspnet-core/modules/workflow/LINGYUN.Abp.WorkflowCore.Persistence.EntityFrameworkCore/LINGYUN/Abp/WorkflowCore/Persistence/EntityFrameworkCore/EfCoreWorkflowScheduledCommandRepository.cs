using LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class EfCoreWorkflowScheduledCommandRepository : EfCoreRepository<WorkflowDbContext, PersistedScheduledCommand, long>, IWorkflowScheduledCommandRepository
    {
        public EfCoreWorkflowScheduledCommandRepository(IDbContextProvider<WorkflowDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> CheckExistsAsync(
            string name,
            string data,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .AnyAsync(x => x.CommandName.Equals(name) && x.Data.Equals(data),
                    GetCancellationToken(cancellationToken));
        }
    }
}
