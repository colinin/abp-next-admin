using LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class EfCoreWorkflowExecutionErrorRepository : EfCoreRepository<WorkflowDbContext, WorkflowExecutionError, int>, IWorkflowExecutionErrorRepository
    {
        public EfCoreWorkflowExecutionErrorRepository(IDbContextProvider<WorkflowDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }
}
