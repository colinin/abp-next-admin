using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

/// <summary>
/// ES DSL表达式翻译器：把 <see cref="Expression{TDelegate}"/> 翻译为
/// Elastic.Clients.Elasticsearch 的 <see cref="Query"/>（QueryDsl）。
/// <para>
/// 支持的算子（超出即抛 <see cref="NotSupportedException"/>，fail loud）：
/// <list type="bullet">
/// <item>逻辑：&amp;&amp;、||、!（映射为 bool filter / should+minimum_should_match / must_not）</item>
/// <item>比较：==、!=、&gt;、&gt;=、&lt;、&lt;=（数值与日期映射为 term / range）</item>
/// <item>null 判断：字段 == null / != null（映射为 must_not exists / exists）</item>
/// <item>字符串：Contains / StartsWith / EndsWith（映射为 wildcard）、Equals（映射为 term）</item>
/// <item>集合：x.Actions.Any(predicate)（映射为 nested 查询或扁平字段展开）</item>
/// <item>常量：true / false（映射为 match_all / match_none）</item>
/// </list>
/// </para>
/// </summary>
internal class AuditLogExpressionQueryTranslator
{
    private bool _actionsIsNested;
    private bool _caseInsensitiveWildcard;
    private bool _appendKeywordForStringEquality;

    public AuditLogExpressionQueryTranslator(
        bool actionsIsNested = false,
        bool caseInsensitiveWildcard = true,
        bool appendKeywordForStringEquality = true)
    {
        _actionsIsNested = actionsIsNested;
        _caseInsensitiveWildcard = caseInsensitiveWildcard;
        _appendKeywordForStringEquality = appendKeywordForStringEquality;
    }

    public Query Translate(Expression<Func<AuditLog, bool>> expression)
    {
        Check.NotNull(expression, nameof(expression));

        return TranslateNode(expression.Body, prefix: null);
    }

    private Query TranslateNode(Expression node, string? prefix)
    {
        return node switch
        {
            ConstantExpression { Value: bool value } =>
                value ? new MatchAllQuery() : new MatchNoneQuery(),
            UnaryExpression { NodeType: ExpressionType.Not } unary =>
                (!TranslateNode(unary.Operand, prefix))!,
            UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary =>
                TranslateNode(unary.Operand, prefix),
            BinaryExpression binary => TranslateBinary(binary, prefix),
            MethodCallExpression method => TranslateMethodCall(method, prefix),
            MemberExpression member when member.Type == typeof(bool) =>
                new TermQuery { Field = ResolveField(member, prefix).Path, Value = true },
            _ => throw new NotSupportedException($"Unsupported expression node {node.NodeType}: {node}"),
        };
    }

    private Query TranslateBinary(BinaryExpression node, string? prefix)
    {
        return node.NodeType switch
        {
            ExpressionType.AndAlso or ExpressionType.And => (Query)new BoolQuery
            {
                Filter = new Query[] { TranslateNode(node.Left, prefix), TranslateNode(node.Right, prefix) },
            }!,
            ExpressionType.OrElse or ExpressionType.Or => (Query)new BoolQuery
            {
                Should = new Query[] { TranslateNode(node.Left, prefix), TranslateNode(node.Right, prefix) },
                MinimumShouldMatch = 1,
            }!,
            ExpressionType.Equal => TranslateComparison(node, prefix),
            ExpressionType.NotEqual => (!TranslateComparison(node, prefix))!,
            ExpressionType.GreaterThan or ExpressionType.GreaterThanOrEqual or ExpressionType.LessThan or ExpressionType.LessThanOrEqual => TranslateComparison(node, prefix),
            _ => throw new NotSupportedException($"Unsupported binary operator {node.NodeType}: {node}"),
        };
    }

    private Query TranslateComparison(BinaryExpression node, string? prefix)
    {
        var (fieldExpression, valueExpression) = ResolveOperands(node);
        var field = ResolveField(fieldExpression, prefix);

        if (IsNullConstant(valueExpression))
        {
            return new BoolQuery
            {
                MustNot = new Query[] { new ExistsQuery { Field = field.Path } },
            };
        }

        var value = Evaluate(valueExpression);

        return value == null
            ? throw new NotSupportedException("The null value is only supported for the == null / != null comparison.")
            : node.NodeType switch
            {
                ExpressionType.Equal => BuildEquality(field, value),
                ExpressionType.GreaterThan => BuildRange(field, greaterThan: value),
                ExpressionType.GreaterThanOrEqual => BuildRange(field, greaterThanOrEqualTo: value),
                ExpressionType.LessThan => BuildRange(field, lessThan: value),
                ExpressionType.LessThanOrEqual => BuildRange(field, lessThanOrEqualTo: value),
                _ => throw new NotSupportedException($"Unsupported comparison operator {node.NodeType}"),
            };
    }

    private static (Expression Field, Expression Value) ResolveOperands(BinaryExpression node)
    {
        var leftIsField = IsFieldLike(node.Left);
        var rightIsField = IsFieldLike(node.Right);
        if (leftIsField == rightIsField)
        {
            throw new NotSupportedException($"The comparison expression must have one side as a field and the other side as a value: {node}");
        }
        return leftIsField ? (node.Left, node.Right) : (node.Right, node.Left);
    }

    private static bool IsFieldLike(Expression expression)
    {
        var current = expression;
        while (current is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
        {
            current = unary.Operand;
        }

        while (current is MemberExpression member)
        {
            current = member.Expression!;
        }

        return current is ParameterExpression;
    }

    private static bool IsNullConstant(Expression expression)
    {
        while (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
        {
            expression = unary.Operand;
        }

        return expression is ConstantExpression { Value: null };
    }

    private Query BuildEquality(FieldRef field, object value)
    {
        if (field.Type == typeof(string))
        {
            var fieldName = _appendKeywordForStringEquality
                ? field.Path + ".keyword"
                : field.Path;
            return new TermQuery
            {
                Field = fieldName,
                Value = (string)value,
                CaseInsensitive = _caseInsensitiveWildcard,
            };
        }

        if (field.Type == typeof(DateTime) || field.Type == typeof(DateTime?))
        {
            var date = (DateTime)value;
            return new DateRangeQuery
            {
                Field = field.Path,
                Gte = date,
                Lte = date,
            };
        }

        return new TermQuery { Field = field.Path, Value = NormalizeValue(value) };
    }

    private static Query BuildRange(
        FieldRef field,
        object? greaterThan = null,
        object? greaterThanOrEqualTo = null,
        object? lessThan = null,
        object? lessThanOrEqualTo = null)
    {
        if (field.Type == typeof(DateTime) || field.Type == typeof(DateTime?))
        {
            var range = new DateRangeQuery { Field = field.Path };
            if (greaterThan != null)
            {
                range.Gt = (DateTime)greaterThan;
            }

            if (greaterThanOrEqualTo != null)
            {
                range.Gte = (DateTime)greaterThanOrEqualTo;
            }

            if (lessThan != null)
            {
                range.Lt = (DateTime)lessThan;
            }

            if (lessThanOrEqualTo != null)
            {
                range.Lte = (DateTime)lessThanOrEqualTo;
            }

            return range;
        }

        var numberRange = new NumberRangeQuery { Field = field.Path };
        if (greaterThan != null)
        {
            numberRange.Gt = ToNumber(greaterThan);
        }

        if (greaterThanOrEqualTo != null)
        {
            numberRange.Gte = ToNumber(greaterThanOrEqualTo);
        }

        if (lessThan != null)
        {
            numberRange.Lt = ToNumber(lessThan);
        }

        if (lessThanOrEqualTo != null)
        {
            numberRange.Lte = ToNumber(lessThanOrEqualTo);
        }

        return numberRange;
    }

    private Query BuildWildcard(string fieldName, string pattern)
    {
        return new WildcardQuery
        {
            Field = fieldName,
            Value = pattern,
            CaseInsensitive = _caseInsensitiveWildcard,
        };
    }

    private Query TranslateMethodCall(MethodCallExpression node, string? prefix)
    {
        if (node.Method.DeclaringType == typeof(Enumerable) && node.Method.Name == nameof(Enumerable.Any))
        {
            var collectionField = ResolveField(node.Arguments[0], prefix);

            Query inner;
            if (node.Arguments.Count == 1)
            {
                inner = new ExistsQuery { Field = collectionField.Path };
            }
            else
            {
                var predicate = UnwrapLambda(node.Arguments[1]);
                inner = TranslateNode(predicate.Body, prefix: collectionField.Path);
            }

            return _actionsIsNested
                ? new NestedQuery(collectionField.Path, inner)
                : inner;
        }

        if (node.Method.DeclaringType == typeof(string) && node.Object != null)
        {
            var field = ResolveField(node.Object, prefix);
            var value = (string)Evaluate(node.Arguments[0])!;
            return node.Method.Name switch
            {
                nameof(string.Contains) => BuildWildcard(field.Path, "*" + EscapeWildcard(value) + "*"),
                nameof(string.StartsWith) => BuildWildcard(field.Path, EscapeWildcard(value) + "*"),
                nameof(string.EndsWith) => BuildWildcard(field.Path, "*" + EscapeWildcard(value)),
                _ => throw new NotSupportedException($"Unsupported string method {node.Method.Name}"),
            };
        }

        if (node.Method.Name == nameof(string.Equals))
        {
            var fieldExpression = node.Object ?? node.Arguments[0];
            var valueExpression = node.Object != null ? node.Arguments[0] : node.Arguments[1];
            var field = ResolveField(fieldExpression, prefix);
            return BuildEquality(field, (string)Evaluate(valueExpression)!);
        }

        throw new NotSupportedException(
            $"Unsupported method invocation {node.Method.DeclaringType?.Name}.{node.Method.Name}");
    }

    private static LambdaExpression UnwrapLambda(Expression expression)
    {
        while (expression is UnaryExpression { NodeType: ExpressionType.Quote } unary)
        {
            expression = unary.Operand;
        }
        return (LambdaExpression)expression;
    }

    private static object? Evaluate(Expression expression)
    {
        return expression is ConstantExpression constant
            ? constant.Value
            : Expression.Lambda(expression).Compile().DynamicInvoke();
    }

    private static string EscapeWildcard(string value)
    {
        return value.Replace("\\", "\\\\").Replace("*", "\\*").Replace("?", "\\?");
    }

    private static FieldValue NormalizeValue(object value)
    {
        return value switch
        {
            string s => s,
            bool b => b,
            int i => i,
            long l => l,
            double d => d,
            Guid g => g.ToString(),
            Enum e => Convert.ToInt64(e),
            _ => Convert.ToDouble(value),
        };
    }

    private static Number? ToNumber(object? value)
    {
        if (value == null)
        {
            return null;
        }
        return value is double d ? d : Convert.ToInt64(value);
    }

    private readonly record struct FieldRef(string Path, Type Type);

    private static FieldRef ResolveField(Expression expression, string? prefix)
    {
        var current = expression;
        while (current is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
        {
            current = unary.Operand;
        }

        var names = new Stack<string>();
        while (current is MemberExpression member)
        {
            names.Push(member.Member.Name);
            current = member.Expression!;
        }

        if (current is not ParameterExpression)
        {
            throw new NotSupportedException($"Unable to parse as field path: {expression}");
        }

        var path = string.Join(".", names);
        if (!string.IsNullOrEmpty(prefix) && path.Length > 0)
        {
            path = prefix + "." + path;
        }
        else if (path.Length == 0)
        {
            path = prefix ?? string.Empty;
        }

        return new FieldRef(path, expression.Type);
    }
}
