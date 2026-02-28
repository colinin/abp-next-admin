using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.EventBus;

[Serializable]
[EventName("abp.background-tasks.job.cleanup_expired")]
public class JobCleanupExpiredEventData : IMultiTenant
{
    public Guid? TenantId { get; set; }
}
