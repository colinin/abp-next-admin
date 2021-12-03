using LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class EfCoreWorkflowScheduledCommandRepository : EfCoreRepository<WorkflowDbContext, WorkflowScheduledCommand, long>, IWorkflowScheduledCommandRepository
    {
        public EfCoreWorkflowScheduledCommandRepository(IDbContextProvider<WorkflowDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }
}
