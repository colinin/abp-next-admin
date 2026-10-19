namespace LINGYUN.Abp.AuditLogging.OpenObserve;

public static class AuditLoggingOpenObserveErrorCodes
{
    public const string Namespace = "AuditLoggingOpenObserve";

    /// <summary>
    /// 当前日志设施不支持删除记录!
    /// </summary>
    public const string DeleteNotSupported = Namespace + ":010001";
}
