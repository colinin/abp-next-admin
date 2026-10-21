using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)类型集合
/// </summary>
/// <remarks>
/// 同一个渠道内的载荷按 <see cref="NotificationChannelDataAttribute.Priority"/> 排序,
/// 匹配顺序必须显式声明, 不允许依赖注册顺序或字段探测顺序。
/// </remarks>
public sealed class NotificationChannelDataDescriptorList : List<NotificationChannelDataDescriptor>
{
}
