namespace LINGYUN.Abp.Identity;
/// <summary>
/// 重复登录策略
/// </summary>
public enum ConcurrentLoginStrategy
{
    /// <summary>
    /// 未定义
    /// </summary>
    None,
    /// <summary>
    /// 限制相同设备登录数量
    /// </summary>
    LogoutFromSameTypeDevicesLimit,
    /// <summary>
    /// 其他相同设备端退出
    /// </summary>
    LogoutFromSameTypeDevices,
    /// <summary>
    /// 其他所有设备退出
    /// </summary>
    LogoutFromAllDevices
}
