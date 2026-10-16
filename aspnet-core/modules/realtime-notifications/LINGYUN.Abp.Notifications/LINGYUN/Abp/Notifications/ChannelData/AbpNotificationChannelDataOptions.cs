using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)选项
/// </summary>
public class AbpNotificationChannelDataOptions
{
    private readonly Dictionary<string, NotificationChannelDataDescriptorList> _providers
        = new(StringComparer.OrdinalIgnoreCase);

    private readonly List<Type> _payloadTypes = new();

    /// <summary>
    /// 已注册的载荷类型
    /// </summary>
    public IReadOnlyList<Type> PayloadTypes => _payloadTypes;

    /// <summary>
    /// 获取指定渠道的载荷集合(按匹配顺序排列)
    /// </summary>
    public NotificationChannelDataDescriptorList GetProviderData(string providerName)
    {
        if (!_providers.TryGetValue(providerName, out var list))
        {
            list = new NotificationChannelDataDescriptorList();
            _providers[providerName] = list;
        }

        return list;
    }

    /// <summary>
    /// 注册渠道数据(载荷)
    /// </summary>
    /// <remarks>
    /// 渠道名称、匹配顺序与存在性判定模式来自
    /// <see cref="NotificationChannelDataAttribute"/>。
    /// </remarks>
    public AbpNotificationChannelDataOptions Add<TChannelData>()
        where TChannelData : NotificationChannelData
    {
        return Add(typeof(TChannelData));
    }

    /// <summary>
    /// 注册渠道数据(载荷)
    /// </summary>
    /// <exception cref="AbpNotificationChannelDataConfigurationException">
    /// 缺少 <see cref="NotificationChannelDataAttribute"/>、
    /// 未声明存在性规则, 或同渠道优先级冲突
    /// </exception>
    public AbpNotificationChannelDataOptions Add(Type channelDataType)
    {
        Check.NotNull(channelDataType, nameof(channelDataType));

        if (_payloadTypes.Contains(channelDataType))
        {
            return this;
        }

        var attribute = channelDataType.GetCustomAttribute<NotificationChannelDataAttribute>(inherit: false);
        if (attribute == null)
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The channel data type '{channelDataType.FullName}' must be annotated with '{nameof(NotificationChannelDataAttribute)}'.");
        }

        // 注册期即校验存在性声明, 让配置错误在启动阶段暴露。
        // 校验只依赖声明元数据, 因此不需要解析任何服务。
        NotificationChannelDataPresence.Validate(channelDataType);

        var descriptors = GetProviderData(attribute.ProviderName);

        if (descriptors.Any(x => x.Priority == attribute.Priority))
        {
            var conflict = descriptors.First(x => x.Priority == attribute.Priority);
            throw new AbpNotificationChannelDataConfigurationException(
                $"The channel data '{channelDataType.FullName}' and '{conflict.PayloadType.FullName}' of provider '{attribute.ProviderName}' declare the same priority '{attribute.Priority}', the matching order would be ambiguous.");
        }

        descriptors.Add(new NotificationChannelDataDescriptor(
            attribute.ProviderName,
            channelDataType,
            attribute.Priority,
            attribute.Presence,
            NotificationChannelDataPresence.Create(channelDataType)));

        descriptors.Sort((left, right) => left.Priority.CompareTo(right.Priority));

        _payloadTypes.Add(channelDataType);

        return this;
    }
}
