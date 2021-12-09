using LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class EfCoreWorkflowEventSubscriptionRepository : EfCoreRepository<WorkflowDbContext, PersistedSubscription, Guid>, IWorkflowEventSubscriptionRepository
    {
        public EfCoreWorkflowEventSubscriptionRepository(IDbContextProvider<WorkflowDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }
    }
}
