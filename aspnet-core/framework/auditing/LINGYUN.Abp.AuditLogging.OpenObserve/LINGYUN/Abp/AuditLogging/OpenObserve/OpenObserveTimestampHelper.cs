using System;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// OpenObserve 时间戳工具
/// </summary>
public static class OpenObserveTimestampHelper
{
    /// <summary>
    /// 转换为微秒时间戳
    /// </summary>
    public static long ToMicroseconds(DateTime time)
    {
        return new DateTimeOffset(time.ToUniversalTime()).ToUnixTimeMilliseconds() * 1000L;
    }

    /// <summary>
    /// 尝试用日志时间覆盖 <c>_timestamp</c>
    /// </summary>
    /// <remarks>
    /// OpenObserve 默认仅允许写入 <c>ZO_INGEST_ALLOWED_UPTO</c>(默认 5)小时内的数据,
    /// 超出窗口的数据会被服务端丢弃, 因此超出时返回 null 由服务端使用写入时间
    /// </remarks>
    /// <param name="time">日志自身的业务时间(如 AuditLog.ExecutionTime / SecurityLog.CreationTime)</param>
    /// <param name="now">当前时间</param>
    /// <param name="allowedUptoHours">允许回溯的小时数, 需与服务端 ZO_INGEST_ALLOWED_UPTO 一致</param>
    public static long? TryGetTimestamp(DateTime time, DateTime now, int allowedUptoHours)
    {
        if (time == default)
        {
            return null;
        }

        if (allowedUptoHours > 0 && time.ToUniversalTime() < now.ToUniversalTime().AddHours(-allowedUptoHours))
        {
            return null;
        }

        return ToMicroseconds(time);
    }
}
