using LINGYUN.Abp.BackgroundTasks;
using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TaskManagement;

[Serializable]
[EventName("abp.tkm.background-job")]
public class BackgroundJobEto : IMultiTenant
{
    public string Id { get; set; } = default!;
    public Guid? TenantId { get; set; }
    public bool IsEnabled { get; set; }
    public string Name { get; set; } = default!;
    public string Group { get; set; } = default!;
    public string? NodeName { get; set; }
    public JobStatus Status { get; set; }
}
