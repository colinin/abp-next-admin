using System.Collections.Generic;

namespace LINGYUN.Abp.Elasticsearch;

/// <summary>
/// Nested 字段的映射信息
/// </summary>
public class NestedMappingInfo
{
    /// <summary>
    /// Nested 字段的完整路径
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Nested 字段名称（最后一段）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Nested 内部的属性映射
    /// </summary>
    public Dictionary<string, FieldMappingInfo> Properties { get; set; } = new();

    /// <summary>
    /// 是否包含指定子字段
    /// </summary>
    public bool ContainsProperty(string propertyName)
    {
        return Properties.ContainsKey(propertyName);
    }

    /// <summary>
    /// 获取子字段信息
    /// </summary>
    public FieldMappingInfo? GetProperty(string propertyName)
    {
        return Properties.GetOrDefault(propertyName);
    }

    /// <summary>
    /// 获取 nested 内部字段的完整路径
    /// </summary>
    public string GetFullPath(string propertyName)
    {
        return $"{Path}.{propertyName}";
    }

    /// <summary>
    /// 获取 nested 内部字段的映射信息（递归）
    /// </summary>
    public FieldMappingInfo? GetNestedField(string fullPath)
    {
        // 去掉当前 nested 路径前缀
        if (!fullPath.StartsWith(Path + "."))
        {
            return null;
        }

        var remainingPath = fullPath.Substring(Path.Length + 1);
        var parts = remainingPath.Split('.');

        FieldMappingInfo? current = null;
        Dictionary<string, FieldMappingInfo>? currentProperties = Properties;

        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i];

            if (currentProperties == null)
            {
                return null;
            }

            if (!currentProperties.TryGetValue(part, out current))
            {
                return null;
            }

            // 如果还有下一级，且当前字段是 object 或 nested 类型
            if (i < parts.Length - 1)
            {
                currentProperties = current.Properties;
            }
        }

        return current;
    }
}
