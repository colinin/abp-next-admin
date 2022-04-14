using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.EventBus;

[Serializable]
[EventName("abp.background-tasks.job")]
public class JobEventData : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string NodeName { get; set; }
    public List<string> IdList { get; set; } = new List<string>();
}
