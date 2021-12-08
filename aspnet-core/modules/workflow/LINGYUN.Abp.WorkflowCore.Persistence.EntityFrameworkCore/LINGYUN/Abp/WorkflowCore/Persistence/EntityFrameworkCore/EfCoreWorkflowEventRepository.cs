using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    public class EfCoreWorkflowEventRepository : EfCoreRepository<WorkflowDbContext, PersistedEvent, Guid>, IWorkflowEventRepository
    {
        public EfCoreWorkflowEventRepository(IDbContextProvider<WorkflowDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }
}
