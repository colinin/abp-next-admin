using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.BlobManagement;

public class AbpBlobManagementOptions
{
    public HashSet<string> StaticContainers { get; }
    public IDictionary<string, IBlobPolicyCheckProvider> BlobPolicyCheckProviders { get; }
    public TimeSpan MinimumTempLifeSpan { get; set; }
    public int MaximumTempSize { get; set; }
    public int CleanupPeriod { get; set; }
    public bool IsCleanupEnabled { get; set; }
    public AbpBlobManagementOptions()
    {
        StaticContainers = new HashSet<string>()
        {
            "public",
            "users",
            "system",
            "workflow",
            "icons",
            "temp"
        };
        BlobPolicyCheckProviders = new Dictionary<string, IBlobPolicyCheckProvider>();

        MinimumTempLifeSpan = TimeSpan.FromHours(6);
        MaximumTempSize = 1000;
        CleanupPeriod = 720_0000;//2h
        IsCleanupEnabled = true;
    }
}
