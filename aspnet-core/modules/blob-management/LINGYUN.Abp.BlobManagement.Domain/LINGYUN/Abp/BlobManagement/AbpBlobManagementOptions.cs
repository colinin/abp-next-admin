using LINGYUN.Abp.BlobManagement.Settings;
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
    /// <summary>
    /// 下载密钥的HMAC签名密钥
    /// </summary>
    /// <remarks>
    /// 未配置时回退到 StringEncryption:DefaultPassPhrase
    /// </remarks>
    public string? DownloadKeySecret { get; set; }
    /// <summary>
    /// 下载密钥有效期
    /// </summary>
    public TimeSpan DownloadKeyLifetime { get; set; }
    /// <summary>
    /// 下载密钥的时间分桶窗口
    /// </summary>
    public TimeSpan DownloadKeyWindow { get; set; }

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
        DownloadKeyLifetime = TimeSpan.FromSeconds(
            BlobManagementSettingNames.DefaultGenerateDownloadUrlExpirySeconds);
        DownloadKeyWindow = TimeSpan.FromSeconds(
            BlobManagementSettingNames.DefaultGenerateDownloadUrlExpirySeconds);
    }
}
