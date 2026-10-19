using System;
using System.Text.Json;

namespace LINGYUN.Abp.Notifications.ChannelData.Tests;

/// <summary>
/// 测试用 JSON 实现
/// </summary>
/// <remarks>
/// 与 <c>AbpNotificationChannelDataJsonSerializer</c> 的职责一致(委托给 JSON 库),
/// 这里直接使用 <see cref="JsonSerializer"/>,
/// 以避免框架契约测试依赖 ABP 的完整模块装载。
/// <para>
/// 渠道特有的多态转换器(模板卡片等)在各自渠道的测试项目中断言。
/// </para>
/// </remarks>
public class TestNotificationChannelDataJsonSerializer : INotificationChannelDataJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new();

    public string Serialize(object value, Type valueType)
    {
        return JsonSerializer.Serialize(value, valueType, Options);
    }

    public object? Deserialize(string json, Type targetType)
    {
        return JsonSerializer.Deserialize(json, targetType, Options);
    }
}
