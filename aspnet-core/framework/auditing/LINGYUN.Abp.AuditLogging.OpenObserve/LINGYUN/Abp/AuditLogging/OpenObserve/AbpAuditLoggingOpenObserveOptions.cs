namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 审计日志 OpenObserve 选项
/// </summary>
/// <remarks>
/// 配置节: <c>AuditLogging:OpenObserve</c>
/// </remarks>
public class AbpAuditLoggingOpenObserveOptions
{
    /// <summary>
    /// 默认审计日志数据流
    /// </summary>
    public const string DefaultAuditLogStream = "audit_log";
    /// <summary>
    /// 默认安全日志数据流
    /// </summary>
    public const string DefaultSecurityLogStream = "security_log";
    /// <summary>
    /// 实体变更数据流后缀
    /// </summary>
    public const string EntityChangeStreamSuffix = "_entity_changes";
    /// <summary>
    /// 操作数据流后缀
    /// </summary>
    public const string ActionStreamSuffix = "_actions";

    /// <summary>
    /// 组织名称
    /// </summary>
    /// <remarks>
    /// 未指定时使用 <c>AbpOpenObserveOptions.Organization</c>
    /// </remarks>
    public string? Organization { get; set; }
    /// <summary>
    /// 审计日志数据流名称
    /// </summary>
    public string AuditLogStream { get; set; }
    /// <summary>
    /// 安全日志数据流名称
    /// </summary>
    public string SecurityLogStream { get; set; }
    /// <summary>
    /// 是否展平写入实体变更
    /// </summary>
    /// <remarks>
    /// 默认 true; 关闭后 <c>IEntityChangeStore</c> 将无法查询到数据
    /// </remarks>
    public bool WriteEntityChanges { get; set; }
    /// <summary>
    /// 是否展平写入操作
    /// </summary>
    /// <remarks>
    /// 默认 true; 关闭后按 <c>Actions.*</c> 条件的查询将无法匹配
    /// </remarks>
    public bool WriteActions { get; set; }
    /// <summary>
    /// 未指定时间范围时的默认查询跨度(天)
    /// </summary>
    /// <remarks>
    /// OpenObserve 查询必须指定时间范围, 默认 365 天
    /// </remarks>
    public int DefaultQueryDays { get; set; }
    /// <summary>
    /// 是否用日志自身的业务时间覆盖 OpenObserve 的 <c>_timestamp</c>
    /// </summary>
    /// <remarks>
    /// 默认 true: 审计日志使用 <c>AuditLog.ExecutionTime</c>, 安全日志使用 <c>SecurityLog.CreationTime</c>,
    /// 实体变更使用 <c>EntityChange.ChangeTime</c>, 操作使用 <c>AuditLogAction.ExecutionTime</c>
    /// </remarks>
    public bool UseLogTimeAsTimestamp { get; set; }
    /// <summary>
    /// 允许覆盖 <c>_timestamp</c> 的最大回溯小时数
    /// </summary>
    /// <remarks>
    /// OpenObserve 默认只接受 <c>ZO_INGEST_ALLOWED_UPTO</c>(默认 5)小时内的数据, 超出窗口会被服务端丢弃,
    /// 因此超出该窗口的日志不再覆盖 <c>_timestamp</c>(改用服务端写入时间); 需与服务端环境变量保持一致
    /// </remarks>
    public int TimestampAllowedUptoHours { get; set; }

    public AbpAuditLoggingOpenObserveOptions()
    {
        AuditLogStream = DefaultAuditLogStream;
        SecurityLogStream = DefaultSecurityLogStream;

        WriteEntityChanges = true;
        WriteActions = true;
        DefaultQueryDays = 365;
        UseLogTimeAsTimestamp = true;
        TimestampAllowedUptoHours = 5;
    }

    /// <summary>
    /// 获取实体变更数据流名称
    /// </summary>
    /// <remarks>
    /// 由 <see cref="AuditLogStream"/> 计算得出: <c>{AuditLogStream}_entity_changes</c>, 无需单独配置
    /// </remarks>
    public virtual string GetEntityChangeStream()
    {
        return $"{AuditLogStream}{EntityChangeStreamSuffix}";
    }

    /// <summary>
    /// 获取操作数据流名称
    /// </summary>
    /// <remarks>
    /// 由 <see cref="AuditLogStream"/> 计算得出: <c>{AuditLogStream}_actions</c>, 无需单独配置;
    /// 大数据量操作(parameters 可能很大)写入独立数据流, 查询时先在该数据流匹配再回主数据流按 id 过滤
    /// </remarks>
    public virtual string GetActionStream()
    {
        return $"{AuditLogStream}{ActionStreamSuffix}";
    }
}
