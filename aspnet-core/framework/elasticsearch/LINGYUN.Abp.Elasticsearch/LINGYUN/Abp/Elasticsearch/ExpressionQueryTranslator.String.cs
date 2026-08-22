using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.Elasticsearch;

public partial class ExpressionQueryTranslator
{
    /// <summary>
    /// 翻译字符串方法
    /// </summary>
    private Query TranslateStringMethod(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        return node.Method.Name switch
        {
            // 实例方法
            nameof(string.Contains) => TranslateStringContains(node, prefix, mappingInfo),
            nameof(string.StartsWith) => TranslateStringStartsWith(node, prefix, mappingInfo),
            nameof(string.EndsWith) => TranslateStringEndsWith(node, prefix, mappingInfo),
            nameof(string.Equals) => TranslateStringEquals(node, prefix, mappingInfo),
            nameof(string.CompareTo) => TranslateStringCompareTo(node, prefix, mappingInfo),
            nameof(string.IndexOf) => TranslateStringIndexOf(node, prefix, mappingInfo),

            // 静态方法
            nameof(string.IsNullOrEmpty) => TranslateStringIsNullOrEmpty(node, prefix, mappingInfo),
            nameof(string.IsNullOrWhiteSpace) => TranslateStringIsNullOrWhiteSpace(node, prefix, mappingInfo),

            // 其他方法不支持
            _ => throw new NotSupportedException($"Unsupported string method {node.Method.Name}"),
        };
    }

    /// <summary>
    /// 翻译 Contains
    /// </summary>
    private Query TranslateStringContains(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (field, value) = GetStringMethodOperands(node, prefix, mappingInfo);
        var fieldMapping = mappingInfo?.GetField(field.Path);
        return TranslateStringContains(field, fieldMapping, value);
    }

    /// <summary>
    /// 翻译 StartsWith
    /// </summary>
    private Query TranslateStringStartsWith(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (field, value) = GetStringMethodOperands(node, prefix, mappingInfo);
        var fieldMapping = mappingInfo?.GetField(field.Path);
        return TranslateStartsWith(field, fieldMapping, value);
    }

    /// <summary>
    /// 翻译 EndsWith
    /// </summary>
    private Query TranslateStringEndsWith(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (field, value) = GetStringMethodOperands(node, prefix, mappingInfo);
        var fieldMapping = mappingInfo?.GetField(field.Path);
        return TranslateEndsWith(field, fieldMapping, value);
    }

    /// <summary>
    /// 翻译 StartsWith
    /// </summary>
    private Query TranslateStartsWith(FieldInfo field, FieldMappingInfo? fieldMapping, string value)
    {
        var pattern = EscapeWildcard(value) + "*";

        // Wildcard 类型
        if (field.IsWildcard || fieldMapping?.IsWildcard == true)
        {
            return new WildcardQuery { Field = field.Path, Value = pattern };
        }

        // Keyword 类型或 Text 有 keyword 子字段
        if (field.IsKeyword || fieldMapping?.IsKeyword == true ||
            (fieldMapping?.IsText == true && fieldMapping?.Properties?.ContainsKey("keyword") == true))
        {
            var fieldPath = fieldMapping?.IsText == true && fieldMapping?.Properties?.ContainsKey("keyword") == true
                ? $"{field.Path}.keyword"
                : field.Path;
            return new WildcardQuery { Field = fieldPath, Value = pattern };
        }

        // Text 类型无 keyword 子字段
        if (field.IsText || fieldMapping?.IsText == true)
        {
            return new MatchPhrasePrefixQuery
            {
                Field = field.Path,
                Query = value
            };
        }

        return new WildcardQuery { Field = field.Path, Value = pattern };
    }

    /// <summary>
    /// 翻译 EndsWith
    /// </summary>
    private Query TranslateEndsWith(FieldInfo field, FieldMappingInfo? fieldMapping, string value)
    {
        var pattern = "*" + EscapeWildcard(value);

        // Wildcard 类型
        if (field.IsWildcard || fieldMapping?.IsWildcard == true)
        {
            return new WildcardQuery { Field = field.Path, Value = pattern };
        }

        // Keyword 类型或 Text 有 keyword 子字段
        if (field.IsKeyword || fieldMapping?.IsKeyword == true ||
            (fieldMapping?.IsText == true && fieldMapping?.Properties?.ContainsKey("keyword") == true))
        {
            var fieldPath = fieldMapping?.IsText == true && fieldMapping?.Properties?.ContainsKey("keyword") == true
                ? $"{field.Path}.keyword"
                : field.Path;
            return new WildcardQuery { Field = fieldPath, Value = pattern };
        }

        // Text 类型无 keyword 子字段
        if (field.IsText || fieldMapping?.IsText == true)
        {
            return new MatchPhraseQuery
            {
                Field = field.Path,
                Query = value
            };
        }

        return new WildcardQuery { Field = field.Path, Value = pattern };
    }

    /// <summary>
    /// 翻译 string.Equals
    /// </summary>
    private Query TranslateStringEquals(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (field, value) = GetStringMethodOperands(node, prefix, mappingInfo);

        if (string.IsNullOrEmpty(value))
        {
            return new BoolQuery { MustNot = new Query[] { new ExistsQuery { Field = field.Path } } };
        }

        return BuildEquality(field, value);
    }

    /// <summary>
    /// 翻译 string.CompareTo
    /// </summary>
    private Query TranslateStringCompareTo(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (field, value) = GetStringMethodOperands(node, prefix, mappingInfo);

        // CompareTo 通常用于比较，这里简化为相等比较
        // 如果需要更复杂的比较逻辑，可以在这里扩展
        return BuildEquality(field, value);
    }

    /// <summary>
    /// 翻译 string.IndexOf
    /// </summary>
    private Query TranslateStringIndexOf(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (field, value) = GetStringMethodOperands(node, prefix, mappingInfo);
        var fieldMapping = mappingInfo?.GetField(field.Path);

        // IndexOf >= 0 等价于 Contains
        return TranslateStringContains(field, fieldMapping, value);
    }

    /// <summary>
    /// 翻译 Contains
    /// </summary>
    private Query TranslateStringContains(FieldInfo field, FieldMappingInfo? fieldMapping, string value)
    {
        // 1. Wildcard 类型 - 直接使用通配符查询（最优）
        if (field.IsWildcard || fieldMapping?.IsWildcard == true)
        {
            return new WildcardQuery
            {
                Field = field.Path,
                Value = "*" + EscapeWildcard(value) + "*"
            };
        }

        // 2. Keyword 类型 - 使用通配符查询
        if (field.IsKeyword || fieldMapping?.IsKeyword == true)
        {
            return new WildcardQuery
            {
                Field = field.Path,
                Value = "*" + EscapeWildcard(value) + "*"
            };
        }

        // 3. Text 类型
        if (field.IsText || fieldMapping?.IsText == true)
        {
            // 3.1 如果有 keyword 子字段，使用 .keyword 进行通配符查询
            if (fieldMapping?.Properties?.ContainsKey("keyword") == true)
            {
                return new WildcardQuery
                {
                    Field = $"{field.Path}.keyword",
                    Value = "*" + EscapeWildcard(value) + "*"
                };
            }

            // 3.2 没有 keyword 子字段，使用 MatchPhrase 进行全文搜索
            // 注意：这不是精确的 Contains，而是分词后的短语匹配
            return new MatchPhraseQuery
            {
                Field = field.Path,
                Query = value
            };
        }

        // 4. 默认 - 尝试使用通配符
        return new WildcardQuery
        {
            Field = field.Path,
            Value = "*" + EscapeWildcard(value) + "*"
        };
    }

    /// <summary>
    /// 翻译 string.IsNullOrEmpty
    /// </summary>
    private Query TranslateStringIsNullOrEmpty(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var field = ResolveField(node.Arguments[0], prefix, mappingInfo);

        // field == null || field == ""
        return new BoolQuery
        {
            Should = new Query[]
            {
                // null 或不存在
                new BoolQuery
                {
                    MustNot = new Query[] { new ExistsQuery { Field = field.Path } }
                },
                // 空字符串
                new TermQuery
                {
                    Field = field.Path,
                    Value = string.Empty
                }
            },
            MinimumShouldMatch = 1
        };
    }

    /// <summary>
    /// 翻译 string.IsNullOrWhiteSpace
    /// </summary>
    private Query TranslateStringIsNullOrWhiteSpace(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var field = ResolveField(node.Arguments[0], prefix, mappingInfo);

        // field == null || field == "" || Regex.IsMatch(field, @"^\s*$")
        return new BoolQuery
        {
            Should = new Query[]
            {
                // null 或不存在
                new BoolQuery
                {
                    MustNot = new Query[] { new ExistsQuery { Field = field.Path } }
                },
                // 空字符串
                new TermQuery
                {
                    Field = field.Path,
                    Value = string.Empty
                },
                // 使用正则表达式匹配只有空白字符的字符串
                new RegexpQuery
                {
                    Field = field.Path,
                    Value = @"^\s*$"
                }
            },
            MinimumShouldMatch = 1
        };
    }

    /// <summary>
    /// 翻译 string.CompareTo 比较
    /// </summary>
    private Query? TranslateStringCompareToComparison(
        MethodCallExpression methodCall,
        Expression constantExpression,
        ExpressionType comparisonType,
        string? prefix,
        IndexMappingInfo? mappingInfo)
    {
        var (field, compareValue) = GetStringMethodOperands(methodCall, prefix, mappingInfo);
        var compareResult = Convert.ToInt32(Evaluate(constantExpression));

        switch (comparisonType)
        {
            case ExpressionType.Equal when compareResult == 0:
                // CompareTo == 0 表示相等
                return BuildEquality(field, compareValue);

            case ExpressionType.NotEqual when compareResult == 0:
                // CompareTo != 0 表示不相等
                return BuildNotEqualQuery(field, compareValue);

            case ExpressionType.GreaterThan when compareResult == 0:
                // CompareTo > 0 表示当前字段值大于比较值
                return BuildRange(field, greaterThan: compareValue);

            case ExpressionType.GreaterThanOrEqual when compareResult == 0:
                // CompareTo >= 0 表示当前字段值大于或等于比较值
                return BuildRange(field, greaterThanOrEqualTo: compareValue);

            case ExpressionType.LessThan when compareResult == 0:
                // CompareTo < 0 表示当前字段值小于比较值
                return BuildRange(field, lessThan: compareValue);

            case ExpressionType.LessThanOrEqual when compareResult == 0:
                // CompareTo <= 0 表示当前字段值小于或等于比较值
                return BuildRange(field, lessThanOrEqualTo: compareValue);

            default:
                return null;
        }
    }

    /// <summary>
    /// 翻译 string.IndexOf 比较
    /// </summary>
    private Query? TranslateStringIndexOfComparison(
        MethodCallExpression methodCall,
        Expression constantExpression,
        ExpressionType comparisonType,
        string? prefix,
        IndexMappingInfo? mappingInfo)
    {
        // 获取字段和搜索值
        var (field, searchValue) = GetStringMethodOperands(methodCall, prefix, mappingInfo);
        var indexResult = Convert.ToInt32(Evaluate(constantExpression));
        var fieldMapping = mappingInfo?.GetField(field.Path);

        switch (comparisonType)
        {
            case ExpressionType.GreaterThanOrEqual when indexResult >= 0:
            case ExpressionType.GreaterThan when indexResult > -1:
            case ExpressionType.NotEqual when indexResult == -1:
                // IndexOf >= 0, IndexOf > -1, IndexOf != -1 表示包含
                return TranslateStringContains(field, fieldMapping, searchValue);

            case ExpressionType.LessThan when indexResult <= 0:
            case ExpressionType.Equal when indexResult == -1:
                // IndexOf == -1, IndexOf < 0 表示不包含
                var containsQuery = TranslateStringContains(field, fieldMapping, searchValue);
                return new BoolQuery
                {
                    MustNot = new Query[] { containsQuery }
                };

            default:
                return null;
        }
    }

    /// <summary>
    /// 翻译字符串实例方法（获取字段和值）
    /// </summary>
    private (FieldInfo Field, string Value) GetStringMethodOperands(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        Expression fieldExpression;
        Expression valueExpression;

        if (node.Object != null)
        {
            // 实例方法：obj.Method(value)
            fieldExpression = node.Object;
            valueExpression = node.Arguments[0];
        }
        else
        {
            // 静态方法：string.Method(field, value)
            fieldExpression = node.Arguments[0];
            valueExpression = node.Arguments.Count > 1 ? node.Arguments[1] : node.Arguments[0];
        }

        var field = ResolveField(fieldExpression, prefix, mappingInfo);
        var value = Evaluate(valueExpression)?.ToString() ?? string.Empty;

        return (field, value);
    }
}
