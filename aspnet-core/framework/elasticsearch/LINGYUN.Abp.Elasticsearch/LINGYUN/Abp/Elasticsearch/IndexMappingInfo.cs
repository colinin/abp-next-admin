using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LINGYUN.Abp.Elasticsearch;
/// <summary>
/// 索引映射信息
/// </summary>
public class IndexMappingInfo
{
    /// <summary>
    /// 索引名称
    /// </summary>
    public string IndexName { get; set; } = string.Empty;

    /// <summary>
    /// 文档类型
    /// </summary>
    public Type? DocumentType { get; set; }

    /// <summary>
    /// 所有字段映射（扁平化）
    /// </summary>
    public Dictionary<string, FieldMappingInfo> Fields { get; set; } = new Dictionary<string, FieldMappingInfo>(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// 按 CLR 属性路径索引的字段映射
    /// </summary>
    public Dictionary<string, FieldMappingInfo> ClrFields { get; set; } = new Dictionary<string, FieldMappingInfo>(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// Nested 字段映射
    /// </summary>
    public Dictionary<string, NestedMappingInfo> NestedFields { get; set; } = new Dictionary<string, NestedMappingInfo>(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// Keyword 字段列表
    /// </summary>
    public HashSet<string> KeywordFields { get; set; } = new();

    /// <summary>
    /// Text 字段列表
    /// </summary>
    public HashSet<string> TextFields { get; set; } = new();
    /// <summary>
    /// Wildcard 字段列表
    /// </summary>
    public HashSet<string> WildcardFields { get; set; } = new();

    /// <summary>
    /// 日期字段列表
    /// </summary>
    public HashSet<string> DateFields { get; set; } = new();

    /// <summary>
    /// 数值字段列表
    /// </summary>
    public HashSet<string> NumericFields { get; set; } = new();

    /// <summary>
    /// 布尔字段列表
    /// </summary>
    public HashSet<string> BooleanFields { get; set; } = new();

    /// <summary>
    /// Nested 字段路径列表
    /// </summary>
    public HashSet<string> NestedFieldPaths { get; set; } = new();

    /// <summary>
    /// 获取字段映射信息
    /// </summary>
    public FieldMappingInfo? GetField(string path)
    {
        return Fields.GetOrDefault(path);
    }

    /// <summary>
    /// 根据 CLR 属性路径获取字段映射信息
    /// </summary>
    public FieldMappingInfo? GetFieldByClrPath(string clrPath)
    {
        return ClrFields.GetOrDefault(clrPath);
    }

    /// <summary>
    /// 根据 CLR 属性表达式获取字段映射信息
    /// </summary>
    public FieldMappingInfo? GetFieldByExpression<TDocument>(Expression<Func<TDocument, object?>> expression)
    {
        var clrPath = GetPropertyPath(expression);
        return GetFieldByClrPath(clrPath);
    }

    /// <summary>
    /// 根据 CLR 属性表达式获取 ES 字段路径
    /// </summary>
    public string? GetElasticsearchFieldPath<TDocument>(Expression<Func<TDocument, object?>> expression)
    {
        var clrPath = GetPropertyPath(expression);
        var field = GetFieldByClrPath(clrPath);
        return field?.Path;
    }

    /// <summary>
    /// 判断是否为 Nested 字段
    /// </summary>
    public bool IsNested(string path)
    {
        return NestedFields.ContainsKey(path) || NestedFieldPaths.Contains(path);
    }

    /// <summary>
    /// 获取 Nested 字段信息
    /// </summary>
    public NestedMappingInfo? GetNestedField(string path)
    {
        return NestedFields.GetOrDefault(path);
    }

    /// <summary>
    /// 获取字段的精确匹配路径（处理 text 的 keyword 子字段）
    /// </summary>
    public string GetExactFieldPath(string path)
    {
        var field = GetField(path);
        if (field == null)
        {
            return path;
        }

        return field.GetKeywordPath();
    }

    /// <summary>
    /// 判断字段是否需要 Nested 查询
    /// </summary>
    public bool ShouldUseNestedQuery(string path)
    {
        // 检查路径本身是否是 Nested
        if (IsNested(path))
        {
            return true;
        }

        // 检查路径的父级是否是 Nested
        var parts = path.Split('.');
        for (var i = 0; i < parts.Length - 1; i++)
        {
            var parentPath = string.Join(".", parts.Take(i + 1));
            if (IsNested(parentPath))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 获取 Nested 字段路径（如果字段在 Nested 内部，返回最近的 Nested 父级路径）
    /// </summary>
    public string? GetNestedParentPath(string path)
    {
        var parts = path.Split('.');
        for (var i = parts.Length - 1; i >= 0; i--)
        {
            var parentPath = string.Join(".", parts.Take(i + 1));
            if (IsNested(parentPath))
            {
                return parentPath;
            }
        }

        return null;
    }
    /// <summary>
    /// 将 CLR 属性路径转换为 ES 字段路径
    /// </summary>
    public string? ConvertClrPathToElasticsearchPath(string clrPath)
    {
        var field = GetFieldByClrPath(clrPath);
        return field?.Path;
    }

    /// <summary>
    /// 将 ES 字段路径转换为 CLR 属性路径
    /// </summary>
    public string? ConvertElasticsearchPathToClrPath(string esPath)
    {
        var field = GetField(esPath);
        return field?.ClrPath;
    }

    private static string GetPropertyPath<TDocument>(Expression<Func<TDocument, object?>> expression)
    {
        var parts = new List<string>();
        var currentExpression = expression.Body;

        while (currentExpression is MemberExpression memberExpression)
        {
            parts.Insert(0, memberExpression.Member.Name);
            currentExpression = memberExpression.Expression!;
        }

        // 处理转换表达式
        if (currentExpression is UnaryExpression unaryExpression &&
            unaryExpression.NodeType == ExpressionType.Convert &&
            unaryExpression.Operand is MemberExpression convertMemberExpression)
        {
            parts.Insert(0, convertMemberExpression.Member.Name);
        }

        return string.Join(".", parts);
    }
}