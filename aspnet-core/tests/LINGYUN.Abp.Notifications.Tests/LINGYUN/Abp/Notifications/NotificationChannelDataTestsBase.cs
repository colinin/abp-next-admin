using LINGYUN.Abp.Tests;
using System.Reflection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 渠道数据测试基类
/// </summary>
/// <remarks>
/// 各渠道的测试项目继承本类并指定自己的测试模块(该模块依赖真实渠道模块),
/// 从而共用同一套"new 消息对象 → <c>IJsonSerializer</c> 序列化 → 渠道侧验证"的写法。
/// </remarks>
public abstract class NotificationChannelDataTestsBase<TStartupModule> : AbpTestsBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    /// <summary>
    /// 渠道侧解析器
    /// </summary>
    protected INotificationChannelDataResolver ChannelDataResolver
        => GetRequiredService<INotificationChannelDataResolver>();

    /// <summary>
    /// 渠道数据存取器(生产侧写入用)
    /// </summary>
    protected INotificationChannelDataAccessor ChannelDataAccessor
        => GetRequiredService<INotificationChannelDataAccessor>();

    /// <summary>
    /// 生产者侧序列化器
    /// </summary>
    protected INotificationChannelDataSerializer ChannelDataSerializer
        => GetRequiredService<INotificationChannelDataSerializer>();

    /// <summary>
    /// ABP 全局 JSON 序列化器
    /// </summary>
    protected IJsonSerializer JsonSerializer
        => GetRequiredService<IJsonSerializer>();

    /// <summary>
    /// 仅序列化消息对象, 便于断言 JSON 契约
    /// </summary>
    protected string Serialize<TPayload>(TPayload payload)
        where TPayload : class
    {
        return JsonSerializer.Serialize(payload);
    }

    /// <summary>
    /// 把消息对象序列化后写入通知数据的载荷键
    /// </summary>
    /// <remarks>
    /// 走生产路径: <see cref="INotificationChannelDataAccessor.Set"/> 对复杂类型
    /// 调用 <see cref="IJsonSerializer"/> 序列化, 与生产者写入的结果完全一致。
    /// </remarks>
    protected NotificationData WriteChannelData<TChannelData>(TChannelData channelData)
        where TChannelData : NotificationChannelData
    {
        return MergeChannelData(new NotificationData(), channelData);
    }

    /// <summary>
    /// 把消息对象序列化后合并写入已有的通知数据
    /// </summary>
    protected NotificationData MergeChannelData<TChannelData>(
        NotificationData notificationData,
        TChannelData channelData)
        where TChannelData : NotificationChannelData
    {
        foreach (var property in typeof(TChannelData).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<NotificationChannelPropertyAttribute>();
            if (attribute == null)
            {
                continue;
            }

            var key = string.IsNullOrWhiteSpace(attribute.Name) ? property.Name : attribute.Name!;

            ChannelDataAccessor.Set(
                notificationData,
                key,
                property.PropertyType,
                property.GetValue(channelData),
                NotificationChannelDataFormat.Auto);
        }

        return notificationData;
    }
}
