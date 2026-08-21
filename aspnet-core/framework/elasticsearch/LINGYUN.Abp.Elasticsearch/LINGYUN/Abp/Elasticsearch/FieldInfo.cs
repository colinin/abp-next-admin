using System;

namespace LINGYUN.Abp.Elasticsearch;
/// <summary>
/// 字段信息
/// </summary>
public record FieldInfo
{
    public string Path { get; init; }
    public Type Type { get; init; }
    public string Name { get; init; }
    public bool IsKeyword { get; init; }
    public bool IsWildcard { get; init; }
    public bool IsText { get; init; }
    public bool IsNested { get; init; }
    public bool IsDate { get; init; }
    public bool IsNumeric { get; init; }
    public bool IsBoolean { get; init; }
    public bool IsRange { get; init; }
    public string? Format { get; init; }
    public bool HasMultiFields { get; init; }

    public FieldInfo(
        string path,
        Type type,
        string name,
        bool isKeyword = false,
        bool isText = false,
        bool isWildcard = false,
        bool isNested = false,
        bool isDate = false,
        bool isNumeric = false,
        bool isBoolean = false,
        bool isRange = false,
        string? format = null,
        bool hasMultiFields = false)
    {
        Path = path;
        Type = type;
        Name = name;
        IsKeyword = isKeyword;
        IsText = isText;
        IsWildcard = isWildcard;
        IsNested = isNested;
        IsDate = isDate;
        IsNumeric = isNumeric;
        IsBoolean = isBoolean;
        IsRange = isRange;
        Format = format;
        HasMultiFields = hasMultiFields;
    }
}
