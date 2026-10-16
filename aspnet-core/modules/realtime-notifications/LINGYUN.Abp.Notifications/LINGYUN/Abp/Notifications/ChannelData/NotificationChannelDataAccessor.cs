using System;
using System.Collections;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 通知渠道数据(载荷)存取器
/// </summary>
/// <remarks>
/// 渠道数据"弱类型属性包 ↔ 强类型值"转换的唯一实现,
/// 由 <see cref="INotificationChannelDataSerializer"/> 与
/// <see cref="INotificationChannelDataResolver"/> 共用, 并保证与所用 JSON 实现无关。
/// </remarks>
public class NotificationChannelDataAccessor : INotificationChannelDataAccessor, ISingletonDependency
{
    protected ILogger<NotificationChannelDataAccessor> Logger { get; }
    protected INotificationChannelDataJsonSerializer JsonSerializer { get; }

    public NotificationChannelDataAccessor(
        INotificationChannelDataJsonSerializer jsonSerializer,
        ILogger<NotificationChannelDataAccessor>? logger = null)
    {
        JsonSerializer = jsonSerializer;
        Logger = logger ?? NullLogger<NotificationChannelDataAccessor>.Instance;
    }

    public virtual string? GetDataTypeKey(Type payloadType)
    {
        return payloadType.FullName;
    }

    public virtual bool IsPresent(NotificationData notificationData, string key)
    {
        return TryGetRawValue(notificationData, key, out _, out _);
    }

    public virtual bool TryGetRawValue(
        NotificationData notificationData,
        string key,
        out object? value,
        out string? matchedKey)
    {
        value = null;
        matchedKey = null;

        if (notificationData == null || key.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (notificationData.ExtraProperties.TryGetValue(key, out var exactValue))
        {
            value = exactValue;
            matchedKey = key;

            return true;
        }

        // 精确匹配未命中时按大小写不敏感匹配, 兼容历史遗留键(如 TemplateId)
        foreach (var property in notificationData.ExtraProperties)
        {
            if (string.Equals(property.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                matchedKey = property.Key;

                return true;
            }
        }

        return false;
    }

    public virtual object? Get(NotificationData notificationData, string key, Type targetType)
    {
        return TryGetRawValue(notificationData, key, out var rawValue, out var matchedKey)
            ? ConvertValue(rawValue!, targetType, matchedKey ?? key)
            : null;
    }

    public virtual TValue? GetValue<TValue>(NotificationData notificationData, string key)
        where TValue : struct
    {
        var value = Get(notificationData, key, typeof(TValue));

        return value is TValue typedValue ? typedValue : null;
    }

    public virtual TValue GetValue<TValue>(NotificationData notificationData, string key, TValue defaultValue)
        where TValue : struct
    {
        return GetValue<TValue>(notificationData, key) ?? defaultValue;
    }

    public virtual string? GetOrNull(NotificationData notificationData, params string[] keys)
    {
        if (notificationData == null || keys == null)
        {
            return null;
        }

        foreach (var key in keys)
        {
            if (TryGetRawValue(notificationData, key, out var value, out _) && value != null)
            {
                return value.ToString();
            }
        }

        return null;
    }

    public virtual void Set(
        NotificationData notificationData,
        string key,
        Type valueType,
        object? value,
        NotificationChannelDataFormat format)
    {
        if (value == null)
        {
            notificationData.TrySetData(key, null);
            return;
        }

        var underlyingType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        if (format == NotificationChannelDataFormat.Json || !NotificationChannelDataValueTypes.IsPrimitive(underlyingType))
        {
            notificationData.TrySetData(key, JsonSerializer.Serialize(value, underlyingType));
            return;
        }

        notificationData.TrySetData(key, value);
    }

    protected virtual object? ConvertValue(object rawValue, Type targetType, string key)
    {
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlyingType.IsInstanceOfType(rawValue))
        {
            return rawValue;
        }

        // 不同 JSON 实现留下的中间表示(JsonElement / JValue)先还原为 CLR 原始值
        var unwrapped = UnwrapJsonValue(rawValue);
        if (unwrapped == null)
        {
            return null;
        }

        if (underlyingType.IsInstanceOfType(unwrapped))
        {
            return unwrapped;
        }

        if (underlyingType == typeof(string))
        {
            return unwrapped.ToString();
        }

        if (underlyingType.IsEnum)
        {
            return ConvertEnumValue(unwrapped, underlyingType, key);
        }

        if (NotificationChannelDataValueTypes.IsPrimitive(underlyingType))
        {
            return ConvertPrimitiveValue(unwrapped, underlyingType, key);
        }

        return ConvertComplexValue(unwrapped, underlyingType, key);
    }

    /// <summary>
    /// 还原 JSON 中间表示
    /// </summary>
    /// <remarks>
    /// System.Text.Json 产生 <c>JsonElement</c>, Newtonsoft 产生 <c>JValue</c>;
    /// 两者都还原为 CLR 原始值, 使转换逻辑与 JSON 实现无关。
    /// </remarks>
    protected virtual object? UnwrapJsonValue(object rawValue)
    {
        if (rawValue is JsonElement jsonElement)
        {
            return UnwrapJsonElement(jsonElement);
        }

        if (rawValue is JValue jValue)
        {
            return jValue.Value;
        }

        return rawValue;
    }

    protected virtual object? UnwrapJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
                return null;

            case JsonValueKind.String:
                return element.GetString();

            case JsonValueKind.True:
                return true;

            case JsonValueKind.False:
                return false;

            case JsonValueKind.Number:
                if (element.TryGetInt32(out var intValue))
                {
                    return intValue;
                }
                if (element.TryGetInt64(out var longValue))
                {
                    return longValue;
                }
                if (element.TryGetDecimal(out var decimalValue))
                {
                    return decimalValue;
                }
                return element.GetDouble();

            default:
                // 数组/对象: 以原始 JSON 文本参与后续反序列化
                return element.GetRawText();
        }
    }

    protected virtual object? ConvertEnumValue(object rawValue, Type underlyingType, string key)
    {
        if (rawValue is string enumName)
        {
            try
            {
                return Enum.Parse(underlyingType, enumName, ignoreCase: true);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex,
                    "Failed to parse the channel data '{Key}' as enum '{Type}', the value will be ignored.",
                    key, underlyingType.FullName);
                return null;
            }
        }

        try
        {
            return Enum.ToObject(
                underlyingType,
                Convert.ChangeType(rawValue, Enum.GetUnderlyingType(underlyingType), CultureInfo.InvariantCulture)!);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex,
                "Failed to convert the channel data '{Key}' to enum '{Type}', the value will be ignored.",
                key, underlyingType.FullName);
            return null;
        }
    }

    protected virtual object? ConvertPrimitiveValue(object rawValue, Type underlyingType, string key)
    {
        // JSON 往返后布尔值可能以数字或字符串形式出现, 而 Convert 只认 "True"/"False"
        if (underlyingType == typeof(bool) && TryConvertBoolean(rawValue, out var booleanValue))
        {
            return booleanValue;
        }

        try
        {
            return Convert.ChangeType(rawValue, underlyingType, CultureInfo.InvariantCulture);
        }
        catch (Exception ex) when (ex is FormatException || ex is InvalidCastException || ex is OverflowException)
        {
            // JSON 往返后字符串可能包裹了引号, 例如 "\"1\"", 再尝试一次去引号转换
            if (rawValue is string text)
            {
                var trimmed = text.Trim().Trim('"');
                if (!ReferenceEquals(trimmed, text) && trimmed.Length > 0)
                {
                    try
                    {
                        return Convert.ChangeType(trimmed, underlyingType, CultureInfo.InvariantCulture);
                    }
                    catch (Exception inner) when (inner is FormatException || inner is InvalidCastException || inner is OverflowException)
                    {
                        // 忽略, 统一在下方记录日志
                    }
                }
            }

            Logger.LogWarning(ex,
                "Failed to convert the channel data '{Key}' of '{ValueType}' to '{TargetType}', the value will be ignored.",
                key, rawValue.GetType().FullName, underlyingType.FullName);
            return null;
        }
    }

    protected virtual object? ConvertComplexValue(object rawValue, Type underlyingType, string key)
    {
        if (rawValue is not string json)
        {
            Logger.LogWarning(
                "The channel data '{Key}' is stored as '{ValueType}', but '{TargetType}' was expected, the value will be ignored.",
                key, rawValue.GetType().FullName, underlyingType.FullName);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize(json, underlyingType);
        }
        catch (Exception ex)
        {
            // 载荷非法时只记录日志并返回 null,
            // 由调用方通过"已声明但解析失败"来明确报错
            Logger.LogWarning(ex,
                "Failed to deserialize the channel data '{Key}' to '{TargetType}', the value will be ignored.",
                key, underlyingType.FullName);
            return null;
        }
    }

    /// <summary>
    /// 尝试把各种表示形式转换为布尔值
    /// </summary>
    /// <remarks>
    /// 数值型 <c>0/1</c> 与字符串 <c>"0"/"1"</c> / <c>"true"/"false"</c> 都会被识别,
    /// <c>Convert.ChangeType</c> 只接受 <c>"True"/"False"</c>, 因此需要单独处理。
    /// </remarks>
    protected virtual bool TryConvertBoolean(object rawValue, out bool result)
    {
        switch (rawValue)
        {
            case bool booleanValue:
                result = booleanValue;
                return true;

            case string text:
                var trimmed = text.Trim().Trim('"');
                if (bool.TryParse(trimmed, out var parsed))
                {
                    result = parsed;
                    return true;
                }
                if (double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
                {
                    result = number != 0;
                    return true;
                }
                break;

            default:
                try
                {
                    result = Convert.ToDouble(rawValue, CultureInfo.InvariantCulture) != 0;
                    return true;
                }
                catch (Exception ex) when (ex is FormatException || ex is InvalidCastException || ex is OverflowException)
                {
                    break;
                }
        }

        result = false;
        return false;
    }
}
