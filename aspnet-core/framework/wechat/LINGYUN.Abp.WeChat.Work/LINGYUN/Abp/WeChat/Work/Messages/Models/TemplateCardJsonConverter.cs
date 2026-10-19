using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LINGYUN.Abp.WeChat.Work.Messages.Models;

/// <summary>
/// 模板卡片消息序列化
/// </summary>
/// <remarks>
/// <para>
/// <see cref="TemplateCard"/> 是抽象类型, Newtonsoft.Json 无法直接实例化它,
/// 导致模板卡片一经序列化(例如作为通知数据持久化、或进入失败重试队列)就再也无法还原:
/// <c>Could not create an instance of type ... Type is an interface or abstract class</c>。
/// </para>
/// <para>
/// 这里按 <c>card_type</c> 判别符还原具体的卡片类型。判别符在构造函数中传递给基类
/// (例如 <c>base("template_card", taskId)</c>), 通过读取构造函数 IL 中的字符串常量获得,
/// 因此新增卡片类型无需修改本转换器。
/// </para>
/// </remarks>
public class TemplateCardJsonConverter : JsonConverter
{
    private const byte LdstrOpCode = 0x72;

    private static readonly Dictionary<string, Type> CardTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

    private static readonly object SyncLock = new object();

    private static bool _initialized;

    /// <summary>
    /// 仅接管抽象卡片类型
    /// </summary>
    /// <remarks>
    /// 具体卡片类型由本转换器显式指定目标类型后递归解析,
    /// 若这里对具体类型也返回 true 会造成无限递归。
    /// </remarks>
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TemplateCard);
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException("The template card json converter does not support writing.");
    }

    public override object? ReadJson(
        JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var jsonObject = JObject.Load(reader);

        var cardType = jsonObject["card_type"]?.ToString();
        var concreteType = ResolveCardType(cardType, objectType);

        if (concreteType == null)
        {
            throw new JsonSerializationException(
                $"Unable to resolve a template card type for card_type: '{cardType}'.");
        }

        using var subReader = jsonObject.CreateReader();

        return serializer.Deserialize(subReader, concreteType);
    }

    /// <summary>
    /// 解析卡片的具体类型
    /// </summary>
    private static Type? ResolveCardType(string? cardType, Type targetType)
    {
        EnsureInitialized();

        if (!cardType.IsNullOrWhiteSpace())
        {
            if (!CardTypes.TryGetValue(cardType!, out var mappedType))
            {
                throw new JsonSerializationException(
                    $"Unknown template card type: '{cardType}'.");
            }

            if (!targetType.IsAssignableFrom(mappedType))
            {
                throw new JsonSerializationException(
                    $"The template card data of card_type: '{cardType}' can not be deserialized as '{targetType.FullName}'.");
            }

            return mappedType;
        }

        // 缺少判别符时, 目标是具体类型则直接使用它
        if (!targetType.IsAbstract)
        {
            return targetType;
        }

        // 抽象目标且判别符缺失时, 按历史行为回退到文本模板卡片
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

            foreach (var type in typeof(TemplateCardJsonConverter).Assembly.GetTypes())
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
