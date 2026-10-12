namespace LINGYUN.Abp.AuditLogging.IP.Location;

/// <summary>
/// 审计日志 IP 定位选项
/// </summary>
/// <remarks>
/// 配置节: <c>AuditLogging:IPLocation</c>
/// (环境变量: <c>AuditLogging__IPLocation__IsEnabled</c>)
/// </remarks>
public class AbpAuditLoggingIPLocationOptions
{
    /// <summary>
    /// 是否启用 IP 定位
    /// </summary>
    /// <remarks>
    /// 默认 <c>false</c>, 需要在宿主配置(例如 docker-compose)中显式开启;
    /// 开启后会把解析结果写入审计日志与安全日志的 <c>Location</c> 属性。
    /// </remarks>
    public bool IsEnabled { get; set; } = false;
}
