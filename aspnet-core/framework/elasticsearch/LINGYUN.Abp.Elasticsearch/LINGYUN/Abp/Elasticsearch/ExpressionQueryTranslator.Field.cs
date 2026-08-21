using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Elasticsearch;

public partial class ExpressionQueryTranslator
{
    /// <summary>
    /// 解析字段
    /// </summary>
    protected virtual FieldInfo ResolveField(Expression expression, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var current = expression;
        while (current is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
        {
            current = unary.Operand;
        }

        var names = new Stack<string>();
        Type? leafType = null;
        string? leafName = null;

        // 收集成员路径
        while (current is MemberExpression member)
        {
            leafName ??= member.Member.Name;
            leafType ??= GetMemberType(member.Member);
            names.Push(ResolveFieldName(member.Member));
            current = member.Expression!;
        }

        if (current is not ParameterExpression && current is not ConstantExpression)
        {
            throw new NotSupportedException($"Unable to parse as field path: {expression}");
        }

        var path = string.Join(".", names);

        // 应用前缀
        if (!string.IsNullOrEmpty(prefix) && path.Length > 0)
        {
            path = prefix + "." + path;
        }
        else if (path.Length == 0)
        {
            path = prefix ?? string.Empty;
        }

        // 获取字段映射信息
        var finalMapping = mappingInfo?.GetField(path);

        // 如果是 text 类型且有 keyword 子字段，自动使用 .keyword
        if (finalMapping?.IsText == true && finalMapping.Properties?.ContainsKey("keyword") == true)
        {
            path = $"{path}.keyword";
            finalMapping = mappingInfo?.GetField(path);
        }

        // 如果 leafType 为 null，使用 expression.Type
        var type = leafType ?? expression.Type;
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return new FieldInfo(
            path,
            underlyingType,
            leafName ?? string.Empty,
            finalMapping?.IsKeyword ?? false,
            finalMapping?.IsText ?? false,
            finalMapping?.IsWildcard ?? false,
            finalMapping?.IsNested ?? false || (mappingInfo?.IsNested(path) ?? false),
            finalMapping?.IsDate ?? false,
            finalMapping?.IsNumeric ?? false,
            finalMapping?.IsBoolean ?? false,
            finalMapping?.IsRange ?? false,
            finalMapping?.Format,
            finalMapping?.HasMultiFields ?? false
        );
    }

    /// <summary>
    /// 获取成员的实际类型
    /// </summary>
    private static Type GetMemberType(MemberInfo member)
    {
        return member switch
        {
            System.Reflection.FieldInfo field => field.FieldType,
            PropertyInfo property => property.PropertyType,
            MethodInfo method => method.ReturnType,
            _ => typeof(object)
        };
    }

    /// <summary>
    /// 解析字段名称
    /// </summary>
    private static string ResolveFieldName(MemberInfo member)
    {
        // 检查 JsonPropertyName 属性
        if (member is PropertyInfo property)
        {
            var jsonName = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonName != null && !string.IsNullOrWhiteSpace(jsonName.Name))
            {
                return jsonName.Name;
            }
        }

        return member.Name;
    }
}
