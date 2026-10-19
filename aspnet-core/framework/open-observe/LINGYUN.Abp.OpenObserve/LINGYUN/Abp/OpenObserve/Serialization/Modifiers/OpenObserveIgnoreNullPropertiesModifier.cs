using System;
using System.Text.Json.Serialization.Metadata;

namespace LINGYUN.Abp.OpenObserve.Serialization.Modifiers;

/// <summary>
/// 忽略数据模型 null 字段的序列化修饰器
/// </summary>
/// <remarks>
public static class OpenObserveIgnoreNullPropertiesModifier
{
    /// <summary>
    /// 本库数据模型的命名空间
    /// </summary>
    public const string ModelsNamespace = "LINGYUN.Abp.OpenObserve.Models";

    public static void Modify(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Type.Namespace == null ||
            !jsonTypeInfo.Type.Namespace.StartsWith(ModelsNamespace, StringComparison.Ordinal))
        {
            return;
        }

        foreach (var property in jsonTypeInfo.Properties)
        {
            property.ShouldSerialize = (_, value) => value != null;
        }
    }
}
