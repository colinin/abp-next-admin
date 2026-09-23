using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Elasticsearch;

public class FieldMappingInfo
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Type? ClrType { get; set; }
    public string ClrPath { get; set; } = string.Empty;

    public bool IsMultiField { get; set; }
    public bool IsKeyword { get; set; }
    public bool IsText { get; set; }
    public bool IsWildcard { get; set; }
    public bool IsNested { get; set; }
    public bool IsObject { get; set; }
    public bool IsDate { get; set; }
    public bool IsNumeric { get; set; }
    public bool IsBoolean { get; set; }
    public bool IsRange { get; set; }

    public string? Format { get; set; }
    public bool? Store { get; set; }
    public bool? Index { get; set; }

    // 子字段（用于 text 的 keyword 子字段，或 object/nested 的内部字段）
    public Dictionary<string, FieldMappingInfo>? Properties { get; set; }
    public Dictionary<string, object>? Meta { get; set; }

    public bool HasMultiFields => Properties?.Count > 0;

    public string GetKeywordPath()
    {
        if (IsKeyword)
        {
            return Path;
        }

        // 如果是 text 类型且有 keyword 子字段
        if (IsText && Properties?.ContainsKey("keyword") == true)
        {
            return $"{Path}.keyword";
        }

        return Path;
    }

    public bool IsComparable => IsDate || IsNumeric || IsRange;
}