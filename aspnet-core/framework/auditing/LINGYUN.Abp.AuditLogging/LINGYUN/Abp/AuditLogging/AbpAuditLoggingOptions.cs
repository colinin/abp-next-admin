namespace LINGYUN.Abp.AuditLogging;
public class AbpAuditLoggingOptions
{
    /// <summary>
    /// 是否启用审计日志记录
    /// </summary>
    public bool IsAuditLogEnabled { get; set; }
    /// <summary>
    /// 使用审计日志队列
    /// </summary>
    /// <remarks>
    /// 不启用队列则直接写入到持久化设施
    /// </remarks>
    public bool UseAuditLogQueue { get; set; }
    /// <summary>
    /// 审计日志最大队列大小
    /// </summary>
    public int MaxAuditLogQueueSize { get; set; }
    /// <summary>
    /// 一次处理审计日志队列大小
    /// </summary>
    public int BatchAuditLogSize { get; set; }
    /// <summary>
    /// 是否启用安全日志记录
    /// </summary>
    public bool IsSecurityLogEnabled { get; set; }
    /// <summary>
    /// 使用安全日志队列
    /// </summary>
    /// <remarks>
    /// 不启用队列则直接写入到持久化设施
    /// </remarks>
    public bool UseSecurityLogQueue { get; set; }
    /// <summary>
    /// 安全日志最大队列大小
    /// </summary>
    public int MaxSecurityLogQueueSize { get; set; }
    /// <summary>
    /// 一次处理安全日志队列大小
    /// </summary>
    public int BatchSecurityLogSize { get; set; }
    public AbpAuditLoggingOptions()
    {
        IsAuditLogEnabled = true;
        UseAuditLogQueue = true;
        BatchAuditLogSize = 100;
        MaxAuditLogQueueSize = 10000;

        IsSecurityLogEnabled = true;
        UseSecurityLogQueue = true;
        BatchSecurityLogSize = 100;
        MaxSecurityLogQueueSize = 10000;
    }
}
