using System;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

public class AbpDistributedLockingDaprOptions
{
    public string StoreName { get; set; }
    public string ResourceId { get; set; }
    public TimeSpan DefaultTimeout { get; set; }

    public AbpDistributedLockingDaprOptions()
    {
        StoreName = "lockstore";
        ResourceId = "dapr-lock-id";
        DefaultTimeout = TimeSpan.FromSeconds(30);
    }
}
