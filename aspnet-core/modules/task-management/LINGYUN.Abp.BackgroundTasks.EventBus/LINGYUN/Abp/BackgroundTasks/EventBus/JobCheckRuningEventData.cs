using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.EventBus;

[Serializable]
[EventName("abp.background-tasks.job.check_running")]
public class JobCheckRuningEventData : IMultiTenant
{
    public Guid? TenantId { get; set; }
}
