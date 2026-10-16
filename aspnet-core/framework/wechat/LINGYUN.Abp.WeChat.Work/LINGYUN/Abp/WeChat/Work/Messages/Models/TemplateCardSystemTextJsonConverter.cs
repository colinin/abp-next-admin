using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;

/// <summary>
/// 模板卡片消息 System.Text.Json 序列化
/// </summary>
/// <remarks>
/// <para>
/// <see cref="TemplateCard"/> 是抽象类型, 默认序列化器无法实例化它,
/// 导致模板卡片一旦被序列化(例如作为通知数据持久化、或进入失败重试队列)
/// 就再也无法还原。
/// </para>
/// <para>
/// 这里按 <c>card_type</c> 判别符还原具体卡片类型。判别符在构造函数中传递给基类
/// (例如 <c>base("template_card", taskId)</c>), 通过读取构造函数 IL 中的字符串常量获得,
/// 因此新增卡片类型无需修改本转换器。
/// </para>
/// <para>
/// 项目使用的 JSON 集成是 <c>Volo.Abp.Json.SystemTextJson</c>,
/// 因此由渠道模块在启动时自动注册到全局选项。
/// </para>
/// </remarks>
public class TemplateCardSystemTextJsonConverter : JsonConverter<TemplateCard>
{
    private const byte LdstrOpCode = 0x72;

    private static readonly Dictionary<string, Type> CardTypes = new(StringComparer.OrdinalIgnoreCase);

    private static readonly object SyncLock = new();

    private static bool _initialized;

    public override TemplateCard? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var jsonElement = jsonDocument.RootElement;

        if (jsonElement.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException(
                $"Unable to resolve a template card from a JSON token of kind '{jsonElement.ValueKind}'.");
        }

        var cardType = jsonElement.TryGetProperty("card_type", out var cardTypeElement)
            ? cardTypeElement.GetString()
            : null;

        var concreteType = ResolveCardType(cardType, typeToConvert);
        if (concreteType == null)
        {
            throw new JsonException($"Unable to resolve a template card type for card_type: '{cardType}'.");
        }

        // 指定具体类型反序列化, 从而避开本转换器(否则会递归)
        return (TemplateCard?)JsonSerializer.Deserialize(jsonElement.GetRawText(), concreteType, options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TemplateCard value,
        JsonSerializerOptions options)
    {
        // 按运行时类型写出, 避免只输出基类成员
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    /// <summary>
    /// 按 <c>card_type</c> 获取具体的卡片类型
    /// </summary>
    public static Type? GetCardType(string cardType)
    {
        EnsureInitialized();

        return CardTypes.TryGetValue(cardType, out var type) ? type : null;
    }

    private static Type? ResolveCardType(string? cardType, Type targetType)
    {
        EnsureInitialized();

        if (!cardType.IsNullOrWhiteSpace())
        {
            if (!CardTypes.TryGetValue(cardType!, out var mappedType))
            {
                throw new JsonException($"Unknown template card type: '{cardType}'.");
            }

            if (!targetType.IsAssignableFrom(mappedType))
            {
                throw new JsonException(
                    $"The template card data of card_type: '{cardType}' can not be deserialized as '{targetType.FullName}'.");
            }

            return mappedType;
        }

        // 缺少判别符时: 目标是具体类型则直接使用
        if (!targetType.IsAbstract)
        {
            return targetType;
        }

        // 抽象目标且判别符缺失: 回退到文本模板卡片
        return CardTypes.TryGetValue(nameof(TextTemplateCard), out var fallbackType) ? fallbackType : null;
    }

    private static void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        lock (SyncLock)
        {
            if (_initialized)
            {
                return;
            }

            foreach (var type in typeof(TemplateCardSystemTextJsonConverter).Assembly.GetTypes())
            {
                if (type.IsAbstract || !typeof(TemplateCard).IsAssignableFrom(type))
                {
                    continue;
                }

                foreach (var discriminator in GetDiscriminators(type))
                {
                    CardTypes[discriminator] = type;
                }
            }

            _initialized = true;
        }
    }

    /// <summary>
    /// 获取卡片类型的判别符
    /// </summary>
    /// <remarks>
    /// 判别符由构造函数传递给基类, 因此读取构造函数 IL 中的字符串常量。
    /// 同时以类型名作为兜底键。
    /// </remarks>
    private static IEnumerable<string> GetDiscriminators(Type cardType)
    {
        var discriminators = new List<string>();

        foreach (var constructor in cardType.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
        {
            try
            {
                var body = constructor.GetMethodBody();
                if (body == null)
                {
                    continue;
                }

                var discriminator = ReadFirstStringConstant(body.GetILAsByteArray(), constructor.Module);
                if (!discriminator.IsNullOrWhiteSpace() && !discriminators.Contains(discriminator!))
                {
                    discriminators.Add(discriminator!);
                }
            }
            catch
            {
                // 读取 IL 失败不影响兜底判别符
            }
        }

        discriminators.Add(cardType.Name);

        return discriminators;
    }

    private static string? ReadFirstStringConstant(byte[]? il, Module module)
    {
        if (il == null)
        {
            return null;
        }

        for (var index = 0; index < il.Length; index++)
        {
            if (il[index] != LdstrOpCode || index + 4 >= il.Length)
            {
                continue;
            }

            var token = BitConverter.ToInt32(il, index + 1);

            try
            {
                return module.ResolveString(token);
            }
            catch
            {
                // 不是字符串元数据标记, 继续扫描
            }
        }

        return null;
    }
}
