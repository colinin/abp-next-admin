using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)解析器
/// </summary>
public class NotificationChannelDataResolver : INotificationChannelDataResolver, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected INotificationChannelDataAccessor Accessor { get; }
    protected INotificationChannelDataPresenceProvider PresenceProvider { get; }
    protected ILogger<NotificationChannelDataResolver> Logger { get; }

    private readonly IOptions<AbpNotificationChannelDataOptions> _options;
    private readonly Dictionary<string, List<ChannelDataRegistration>> _registrations;
    private readonly Dictionary<Type, ChannelDataRegistration> _registrationsByType;
    private readonly object _syncLock = new();

    public NotificationChannelDataResolver(
        IServiceProvider serviceProvider,
        IOptions<AbpNotificationChannelDataOptions> options,
        INotificationChannelDataAccessor accessor,
        INotificationChannelDataPresenceProvider presenceProvider,
        ILogger<NotificationChannelDataResolver>? logger = null)
    {
        ServiceProvider = serviceProvider;
        _options = options;
        Accessor = accessor;
        PresenceProvider = presenceProvider;
        Logger = logger ?? NullLogger<NotificationChannelDataResolver>.Instance;
        _registrations = new Dictionary<string, List<ChannelDataRegistration>>(StringComparer.OrdinalIgnoreCase);
        _registrationsByType = new Dictionary<Type, ChannelDataRegistration>();
    }

    public virtual IReadOnlyList<NotificationChannelData> Resolve(string providerName, NotificationData notificationData)
    {
        Check.NotNull(providerName, nameof(providerName));
        Check.NotNull(notificationData, nameof(notificationData));

        var result = new List<NotificationChannelData>();

        foreach (var registration in GetRegistrations(providerName))
        {
            if (!registration.IsPresent(notificationData))
            {
                continue;
            }

            var channelData = CreateChannelData(registration, notificationData);
            if (channelData != null)
            {
                result.Add(channelData);
            }
        }

        return result;
    }

    public virtual TChannelData? Resolve<TChannelData>(NotificationData notificationData)
        where TChannelData : NotificationChannelData
    {
        return Resolve(typeof(TChannelData), notificationData) as TChannelData;
    }

    public virtual NotificationChannelData? Resolve(Type payloadType, NotificationData notificationData)
    {
        Check.NotNull(payloadType, nameof(payloadType));
        Check.NotNull(notificationData, nameof(notificationData));

        var registration = GetRegistration(payloadType);

        return registration.IsPresent(notificationData)
            ? CreateChannelData(registration, notificationData)
            : null;
    }

    public virtual bool IsDeclared(NotificationData notificationData, Type payloadType)
    {
        Check.NotNull(notificationData, nameof(notificationData));
        Check.NotNull(payloadType, nameof(payloadType));

        return GetRegistration(payloadType).IsPresent(notificationData);
    }

    public virtual IReadOnlyList<Type> GetPayloadTypes(string providerName)
    {
        return GetRegistrations(providerName)
            .Select(x => x.Descriptor.PayloadType)
            .ToArray();
    }

    public virtual string? GetKey(NotificationData notificationData, Type payloadType)
    {
        Check.NotNull(payloadType, nameof(payloadType));

        // 返回数据中实际命中的载荷标识键(大小写可能与声明不同), 用于诊断信息
        foreach (var key in PresenceProvider.GetMatchingKeys(payloadType))
        {
            if (Accessor.TryGetRawValue(notificationData, key, out _, out var matchedKey))
            {
                return matchedKey ?? key;
            }
        }

        // 数据中不存在时返回声明的标识键
        return PresenceProvider.GetMatchingKeys(payloadType).FirstOrDefault();
    }

    public virtual object? Get(NotificationData notificationData, string key, Type targetType)
    {
        return Accessor.Get(notificationData, key, targetType);
    }

    public virtual TValue? GetValue<TValue>(NotificationData notificationData, string key)
        where TValue : struct
    {
        return Accessor.GetValue<TValue>(notificationData, key);
    }

    public virtual TValue GetValue<TValue>(NotificationData notificationData, string key, TValue defaultValue)
        where TValue : struct
    {
        return Accessor.GetValue(notificationData, key, defaultValue);
    }

    public virtual string? GetOrNull(NotificationData notificationData, params string[] keys)
    {
        return Accessor.GetOrNull(notificationData, keys);
    }

    protected virtual ChannelDataRegistration GetRegistration(Type payloadType)
    {
        if (_registrationsByType.TryGetValue(payloadType, out var registration))
        {
            return registration;
        }

        // 未注册的类型按约定直接读取, 避免生产者与渠道耦合到注册顺序
        return new ChannelDataRegistration(
            new NotificationChannelDataDescriptor(
                string.Empty,
                payloadType,
                0,
                PresenceProvider.GetPresenceMode(payloadType),
                PresenceProvider.Create(payloadType)),
            serviceProvider => ActivatorUtilities.CreateInstance(serviceProvider, payloadType));
    }

    protected virtual NotificationChannelData? CreateChannelData(
        ChannelDataRegistration registration,
        NotificationData notificationData)
    {
        object? instance;
        try
        {
            instance = registration.Factory(ServiceProvider);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                "Failed to create the channel data '{ChannelData}' for provider '{Provider}'.",
                registration.Descriptor.PayloadType.FullName, registration.Descriptor.ProviderName);
            return null;
        }

        if (instance is not NotificationChannelData channelData)
        {
            Logger.LogError(
                "The channel data '{ChannelData}' for provider '{Provider}' is not assignable to '{BaseType}'.",
                registration.Descriptor.PayloadType.FullName, registration.Descriptor.ProviderName, typeof(NotificationChannelData).FullName);
            return null;
        }

        foreach (var property in NotificationChannelDataPropertyCache.GetProperties(registration.Descriptor.PayloadType))
        {
            // 按大小写不敏感的方式定位实际键名, 兼容生产者使用不同大小写写入的数据
            if (!Accessor.TryGetRawValue(notificationData, property.Key, out var rawValue, out var matchedKey))
            {
                continue;
            }

            if (rawValue == null)
            {
                continue;
            }

            var value = ConvertPropertyValue(notificationData, property, matchedKey);

            if (value == null)
            {
                // 值存在但无法解析/类型不符, 说明生产者写入的载荷非法
                Logger.LogWarning(
                    "The channel data '{ChannelData}' property '{Property}' of provider '{Provider}' can not be resolved from the key '{Key}'.",
                    registration.Descriptor.PayloadType.FullName, property.Key, registration.Descriptor.ProviderName, matchedKey ?? property.Key);
                return null;
            }

            property.SetValue(channelData, value);
        }

        return channelData;
    }

    /// <summary>
    /// 转换载荷属性值
    /// </summary>
    /// <returns>转换结果; 无法转换或类型不符时返回 null</returns>
    protected virtual object? ConvertPropertyValue(
        NotificationData notificationData,
        NotificationChannelDataProperty property,
        string? matchedKey)
    {
        var value = Accessor.Get(notificationData, matchedKey ?? property.Key, property.PropertyType);

        if (value == null)
        {
            return null;
        }

        // 值的运行时类型必须与属性声明类型兼容
        var underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        return underlyingType.IsInstanceOfType(value) ? value : null;
    }

    protected virtual IReadOnlyList<ChannelDataRegistration> GetRegistrations(string providerName)
    {
        lock (_syncLock)
        {
            if (_registrations.TryGetValue(providerName, out var registrations))
            {
                return registrations;
            }

            registrations = _options.Value
                .GetProviderData(providerName)
                .Select(CreateRegistration)
                .ToList();

            _registrations[providerName] = registrations;

            return registrations;
        }
    }

    protected virtual ChannelDataRegistration CreateRegistration(NotificationChannelDataDescriptor descriptor)
    {
        return new ChannelDataRegistration(
            descriptor,
            serviceProvider => ActivatorUtilities.CreateInstance(serviceProvider, descriptor.PayloadType));
    }

    /// <summary>
    /// 渠道数据注册项
    /// </summary>
    protected sealed class ChannelDataRegistration
    {
        public NotificationChannelDataDescriptor Descriptor { get; }
        public Func<IServiceProvider, object> Factory { get; }
        public Func<NotificationData, bool> IsPresent => Descriptor.IsPresent;

        public ChannelDataRegistration(
            NotificationChannelDataDescriptor descriptor,
            Func<IServiceProvider, object> factory)
        {
            Descriptor = descriptor;
            Factory = factory;
        }
    }
}
