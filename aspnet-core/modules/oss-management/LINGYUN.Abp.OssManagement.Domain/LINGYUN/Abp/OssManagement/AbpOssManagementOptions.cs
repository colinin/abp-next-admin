using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;

namespace LINGYUN.Abp.OssManagement;

public class AbpOssManagementOptions
{
    /// <summary>
    /// 静态容器
    /// 不允许被删除
    /// </summary>
    public List<string> StaticBuckets { get; }
    /// <summary>
    /// Default value: true.
    /// If <see cref="AbpBackgroundWorkerOptions.IsEnabled"/> is false,
    /// this property is ignored and the cleanup worker doesn't work for this application instance.
    /// </summary>
    public bool IsCleanupEnabled { get; set; }
    /// <summary>
    /// Default: 3,600,000 ms.
    /// </summary>
    public int CleanupPeriod { get; set; }
    /// <summary>
    /// 禁用缓存目录清除作业
    /// default: false
    /// </summary>
    public bool DisableTempPruning { get; set; }
    /// <summary>
    /// 每批次清理数量
    /// default: 100
    /// </summary>
    public int MaximumTempSize { get; set; }
    /// <summary>
    /// 最小缓存对象寿命
    /// default: 30 minutes
    /// </summary>
    public TimeSpan MinimumTempLifeSpan { get; set; }
    /// <summary>
    /// 文件流处理器
    /// </summary>
    [NotNull]
    public List<IOssObjectProcesserContributor> Processers { get; }

    public AbpOssManagementOptions()
    {
        StaticBuckets = new List<string>
        {
            // 公共目录
            "public",
            // 用户私有目录
            "users",
            // 系统目录
            "system",
            // 工作流
            "workflow",
            // 图标
            "icons",
            // 缓存
            "temp"
        };
        Processers = new List<IOssObjectProcesserContributor>
        {
            new NoneOssObjectProcesser()
        };

        IsCleanupEnabled = true;
        CleanupPeriod = 3_600_000;
        MaximumTempSize = 100;
        DisableTempPruning = false;
        MinimumTempLifeSpan = TimeSpan.FromMinutes(30);
    }

    public void AddStaticBucket(string bucket)
    {
        Check.NotNullOrWhiteSpace(bucket, nameof(bucket));

        StaticBuckets.AddIfNotContains(bucket);
    }

    public bool CheckStaticBucket(string bucket)
    {
        return StaticBuckets.Any(bck => bck.Equals(bucket));
    }
}
