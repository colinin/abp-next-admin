using System;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据配置异常
/// </summary>
/// <remarks>
/// 注册期契约校验失败时抛出, 目的: 让配置错误在启动阶段暴露,
/// 而不是在生产环境静默丢弃通知。
/// </remarks>
public class AbpNotificationChannelDataConfigurationException : Exception
{
    public AbpNotificationChannelDataConfigurationException(string message)
        : base(message)
    {
    }

    public AbpNotificationChannelDataConfigurationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
