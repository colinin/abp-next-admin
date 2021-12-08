using System;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public interface IWorkflowEventSubscriptionRepository : IRepository<PersistedSubscription, Guid>
    {
    }
}
