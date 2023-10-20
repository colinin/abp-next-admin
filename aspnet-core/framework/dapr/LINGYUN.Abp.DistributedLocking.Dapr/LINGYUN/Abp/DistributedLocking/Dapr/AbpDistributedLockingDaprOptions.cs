using System;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

public class AbpDistributedLockingDaprOptions
{
    /// <summary>
    ///  The name of the lock store to be queried.
    /// </summary>
    /// <remarks>
    /// default: lockstore
    /// </remarks>
    public string StoreName { get; set; }
    /// <summary>
    /// Indicates the identifier of lock owner.
    /// </summary>
    /// <remarks>
    /// default: dapr-lock-owner
    /// </remarks>
    public string DefaultIdentifier { get; set; }
    /// <summary>
    /// The time after which the lock gets expired.
    /// </summary>
    /// <remarks>
    /// default: 30 seconds
    /// </remarks>
    public TimeSpan DefaultTimeout { get; set; }

    public AbpDistributedLockingDaprOptions()
    {
        StoreName = "lockstore";
        DefaultIdentifier = "dapr-lock-owner";
        DefaultTimeout = TimeSpan.FromSeconds(30);
    }
}
