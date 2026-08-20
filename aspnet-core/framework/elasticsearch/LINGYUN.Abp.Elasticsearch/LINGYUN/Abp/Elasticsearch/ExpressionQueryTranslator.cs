using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch;

/// <summary>
/// 表达式查询转换器 - 将 LINQ 表达式转换为 Elasticsearch Query
/// </summary>
public class ExpressionQueryTranslator : IExpressionQueryTranslator, ISingletonDependency
{
    private readonly IIndexMappingProvider _indexMappingProvider;

    public ExpressionQueryTranslator(
        IIndexMappingProvider indexMappingProvider)
    {
        _indexMappingProvider = indexMappingProvider;
    }

    /// <summary>
    /// 翻译表达式
    /// </summary>
    public async virtual Task<Query> TranslateAsync<TDocument>(string indexName, Expression<Func<TDocument, bool>> expression)
    {
        Check.NotNullOrWhiteSpace(indexName, nameof(indexName));
        Check.NotNull(expression, nameof(expression));

        var indexMapping = await _indexMappingProvider.GetMappingAsync(indexName);

        return TranslateNode(expression.Body, prefix: null, indexMapping);
    }

    #region 节点翻译

    /// <summary>
    /// 翻译表达式节点
    /// </summary>
    protected virtual Query TranslateNode(Expression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        return node switch
        {
            ConstantExpression { Value: bool value } =>
                value ? new MatchAllQuery() : new MatchNoneQuery(),

            UnaryExpression { NodeType: ExpressionType.Not } unary =>
                Negate(TranslateNode(unary.Operand, prefix, mappingInfo)),

            UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary =>
                TranslateNode(unary.Operand, prefix, mappingInfo),

            BinaryExpression binary => TranslateBinary(binary, prefix, mappingInfo),

            MethodCallExpression method => TranslateMethodCall(method, prefix, mappingInfo),

            MemberExpression member when IsHasValueAccess(member) =>
                new ExistsQuery { Field = ResolveField(member.Expression!, prefix, mappingInfo).Path },

            MemberExpression member when member.Type == typeof(bool) =>
                new TermQuery { Field = ResolveField(member, prefix, mappingInfo).Path, Value = true },

            MemberExpression member when IsNullableHasValue(member) =>
                new ExistsQuery { Field = ResolveField(member.Expression!, prefix, mappingInfo).Path },

            MemberExpression member when IsNullableValueAccess(member) =>
                TranslateNode(member.Expression!, prefix, mappingInfo),

            _ => throw new NotSupportedException($"Unsupported expression node {node.NodeType}: {node}"),
        };
    }
    #endregion

    #region 二元表达式翻译

    /// <summary>
    /// 翻译二元表达式
    /// </summary>
    protected virtual Query TranslateBinary(BinaryExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        return node.NodeType switch
        {
            ExpressionType.AndAlso or ExpressionType.And => TranslateAndAlso(node, prefix, mappingInfo),

            ExpressionType.OrElse or ExpressionType.Or => TranslateOrElse(node, prefix, mappingInfo),

            ExpressionType.Equal => TranslateComparison(node, prefix, mappingInfo),

            ExpressionType.NotEqual => TranslateComparison(node, prefix, mappingInfo),

            ExpressionType.GreaterThan or ExpressionType.GreaterThanOrEqual or
            ExpressionType.LessThan or ExpressionType.LessThanOrEqual => TranslateComparison(node, prefix, mappingInfo),

            _ => throw new NotSupportedException($"Unsupported binary operator {node.NodeType}: {node}"),
        };
    }


    /// <summary>
    /// 构建 And 查询（展平 BoolQuery）
    /// </summary>
    private Query TranslateAndAlso(BinaryExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var filters = new List<Query>();

        // 收集左侧所有 Filter
        CollectFilters(node.Left, prefix, mappingInfo, filters);

        // 收集右侧 Filter
        CollectFilters(node.Right, prefix, mappingInfo, filters);

        return new BoolQuery
        {
            Filter = filters.ToArray()
        };
    }

    /// <summary>
    /// 递归收集 Filter 查询
    /// </summary>
    private void CollectFilters(Expression node, string? prefix, IndexMappingInfo? mappingInfo, List<Query> filters)
    {
        if (node is BinaryExpression binary &&
            (binary.NodeType == ExpressionType.AndAlso || binary.NodeType == ExpressionType.And))
        {
            // 递归收集左右子节点
            CollectFilters(binary.Left, prefix, mappingInfo, filters);
            CollectFilters(binary.Right, prefix, mappingInfo, filters);
        }
        else
        {
            // 非 And 表达式，直接翻译并添加到列表
            var query = TranslateNode(node, prefix, mappingInfo);

            // 如果翻译结果是 BoolQuery 且有 Filter，展平它
            if (query.Bool != null && query.Bool.Filter != null && query.Bool.Filter.Count > 0)
            {
                foreach (var subQuery in query.Bool.Filter)
                {
                    filters.Add(subQuery);
                }
            }
            else if (query.Bool != null && query.Bool.Must != null && query.Bool.Must.Count > 0)
            {
                foreach (var subQuery in query.Bool.Must)
                {
                    filters.Add(subQuery);
                }
            }
            else
            {
                filters.Add(query);
            }
        }
    }

    /// <summary>
    /// 构建 Or 查询（展平 BoolQuery）
    /// </summary>
    private Query TranslateOrElse(BinaryExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var shouldQueries = new List<Query>();

        CollectShouldQueries(node.Left, prefix, mappingInfo, shouldQueries);
        CollectShouldQueries(node.Right, prefix, mappingInfo, shouldQueries);

        return new BoolQuery
        {
            Should = shouldQueries.ToArray(),
            MinimumShouldMatch = 1
        };
    }

    /// <summary>
    /// 递归收集 Should 查询
    /// </summary>
    private void CollectShouldQueries(Expression node, string? prefix, IndexMappingInfo? mappingInfo, List<Query> shouldQueries)
    {
        if (node is BinaryExpression binary &&
            (binary.NodeType == ExpressionType.OrElse || binary.NodeType == ExpressionType.Or))
        {
            CollectShouldQueries(binary.Left, prefix, mappingInfo, shouldQueries);
            CollectShouldQueries(binary.Right, prefix, mappingInfo, shouldQueries);
        }
        else
        {
            var query = TranslateNode(node, prefix, mappingInfo);

            // 如果翻译结果是 BoolQuery 且有 Should，展平它
            if (query.Bool != null && query.Bool.Should != null && query.Bool.Should.Count > 0)
            {
                foreach (var subQuery in query.Bool.Should)
                {
                    shouldQueries.Add(subQuery);
                }
            }
            else
            {
                shouldQueries.Add(query);
            }
        }
    }

    /// <summary>
    /// 翻译比较表达式
    /// </summary>
    protected virtual Query TranslateComparison(BinaryExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var (fieldExpression, valueExpression) = ResolveOperands(node);
        var field = ResolveField(fieldExpression, prefix, mappingInfo);

        // 处理 null 比较
        if (IsNullConstant(valueExpression))
        {
            return node.NodeType == ExpressionType.NotEqual
                ? new ExistsQuery { Field = field.Path }
                : new BoolQuery
                {
                    MustNot = new Query[] { new ExistsQuery { Field = field.Path } }
                };
        }

        var value = Evaluate(valueExpression);

        if (value == null)
        {
            throw new NotSupportedException("The null value is only supported for the == null / != null comparison.");
        }

        // 处理集合包含
        if (field.Type.IsArray || (field.Type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(field.Type)))
        {
            return BuildTermsQuery(field, value);
        }

        return node.NodeType switch
        {
            ExpressionType.Equal => BuildEquality(field, value),
            ExpressionType.NotEqual => BuildNotEqualQuery(field, value),
            ExpressionType.GreaterThan => BuildRange(field, greaterThan: value),
            ExpressionType.GreaterThanOrEqual => BuildRange(field, greaterThanOrEqualTo: value),
            ExpressionType.LessThan => BuildRange(field, lessThan: value),
            ExpressionType.LessThanOrEqual => BuildRange(field, lessThanOrEqualTo: value),
            _ => throw new NotSupportedException($"Unsupported comparison operator {node.NodeType}"),
        };
    }

    /// <summary>
    /// 解析操作数，确定字段和值
    /// </summary>
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

    /// <summary>
    /// 判断表达式是否为字段
    /// </summary>
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

    /// <summary>
    /// 判断表达式是否为 null 常量
    /// </summary>
    private static bool IsNullConstant(Expression expression)
    {
        while (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
        {
            expression = unary.Operand;
        }

        return expression is ConstantExpression { Value: null };
    }

    #endregion

    #region 方法调用翻译

    /// <summary>
    /// 取反查询
    /// </summary>
    private static Query Negate(Query query)
    {
        // 检查是否为 BoolQuery（通过 Bool 属性）
        if (query.Bool != null)
        {
            var boolQuery = query.Bool;
            var newBool = new BoolQuery();

            // MustNot -> Must
            if (boolQuery.MustNot != null && boolQuery.MustNot.Count > 0)
            {
                newBool.Must = boolQuery.MustNot.ToArray();
                return newBool;
            }

            // Must -> MustNot
            if (boolQuery.Must != null && boolQuery.Must.Count > 0)
            {
                newBool.MustNot = boolQuery.Must.ToArray();
                return newBool;
            }

            // Filter -> MustNot
            if (boolQuery.Filter != null && boolQuery.Filter.Count > 0)
            {
                newBool.MustNot = boolQuery.Filter.ToArray();
                return newBool;
            }

            // Should -> MustNot
            if (boolQuery.Should != null && boolQuery.Should.Count > 0)
            {
                newBool.MustNot = boolQuery.Should.ToArray();
                return newBool;
            }

            // 空 BoolQuery -> MatchAll
            return new MatchAllQuery();
        }

        // 对于 ExistsQuery，取反后变成 MustNot + Exists
        if (query.Exists != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.Exists }
            };
        }

        // 对于 MatchAllQuery，取反后变成 MatchNoneQuery
        if (query.MatchAll != null)
        {
            return new MatchNoneQuery();
        }

        // 对于 MatchNoneQuery，取反后变成 MatchAllQuery
        if (query.MatchNone != null)
        {
            return new MatchAllQuery();
        }

        // 对于 TermQuery，取反后变成 MustNot + Term
        if (query.Term != null)
        {
            if (query.Term.Value.IsBool && query.Term.Value.TryGetBool(out var boolValue))
            {
                return new TermQuery
                {
                    Field = query.Term.Field,
                    Value = FieldValue.FromValue(!boolValue)
                };
            }
            return new BoolQuery
            {
                MustNot = new Query[] { query.Term }
            };
        }

        // 对于 WildcardQuery，取反后变成 MustNot + Wildcard
        if (query.Wildcard != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.Wildcard }
            };
        }

        // 对于 MatchQuery，取反后变成 MustNot + Match
        if (query.Match != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.Match }
            };
        }

        // 对于 MatchPhraseQuery，取反后变成 MustNot + MatchPhrase
        if (query.MatchPhrase != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.MatchPhrase }
            };
        }

        // 对于 MatchPhrasePrefixQuery，取反后变成 MustNot + MatchPhrasePrefix
        if (query.MatchPhrasePrefix != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.MatchPhrasePrefix }
            };
        }

        // 对于 NumberRangeQuery
        if (query.Range != null && query.Range is NumberRangeQuery numberRange)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { numberRange }
            };
        }

        // 对于 DateRangeQuery
        if (query.Range != null && query.Range is DateRangeQuery dateRange)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { dateRange }
            };
        }

        // 对于 TermsQuery
        if (query.Terms != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.Terms }
            };
        }

        // 对于 NestedQuery
        if (query.Nested != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.Nested }
            };
        }

        // 对于 ScriptQuery
        if (query.Script != null)
        {
            return new BoolQuery
            {
                MustNot = new Query[] { query.Script }
            };
        }

        // 默认：对于未知类型，使用 MustNot 包装
        return new BoolQuery
        {
            MustNot = new Query[] { query }
        };
    }

    /// <summary>
    /// 翻译方法调用表达式
    /// </summary>
    protected virtual Query TranslateMethodCall(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        // Enumerable.Any / Enumerable.Contains / Enumerable.All
        if (node.Method.DeclaringType == typeof(Enumerable))
        {
            return TranslateEnumerableMethod(node, prefix, mappingInfo);
        }

        // string.Equals 需放在其他方法前
        if (node.Method.Name == nameof(string.Equals))
        {
            return TranslateStringEquals(node, prefix, mappingInfo);
        }

        // 字符串方法
        if (node.Method.DeclaringType == typeof(string) && node.Object != null)
        {
            return TranslateStringMethod(node, prefix, mappingInfo);
        }

        // Enum.HasFlag
        if (node.Method.Name == nameof(Enum.HasFlag))
        {
            return TranslateEnumHasFlag(node, prefix, mappingInfo);
        }

        // List/Collection.Contains (实例方法)
        if (node.Method.Name == nameof(ICollection<>.Contains) && node.Object != null)
        {
            return TranslateCollectionContains(node, prefix, mappingInfo);
        }

        throw new NotSupportedException(
            $"Unsupported method invocation {node.Method.DeclaringType?.Name}.{node.Method.Name}");
    }

    /// <summary>
    /// 翻译 Enumerable 方法
    /// </summary>
    private Query TranslateEnumerableMethod(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        switch (node.Method.Name)
        {
            case nameof(Enumerable.Any):
                return TranslateEnumerableAny(node, prefix, mappingInfo);

            case nameof(Enumerable.Contains):
                return TranslateEnumerableContains(node, prefix, mappingInfo);

            case nameof(Enumerable.All):
                return TranslateEnumerableAll(node, prefix, mappingInfo);

            default:
                throw new NotSupportedException($"Unsupported Enumerable method {node.Method.Name}");
        }
    }

    /// <summary>
    /// 翻译 Enumerable.Any
    /// </summary>
    private Query TranslateEnumerableAny(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var collectionField = ResolveField(node.Arguments[0], prefix, mappingInfo);

        Query inner;
        if (node.Arguments.Count == 1)
        {
            // .Any() 检查集合是否存在
            inner = new ExistsQuery { Field = collectionField.Path };
        }
        else
        {
            // .Any(predicate)
            var predicate = UnwrapLambda(node.Arguments[1]);
            inner = TranslateNode(predicate.Body, prefix: collectionField.Path, mappingInfo);
        }

        var shouldUseNested = collectionField.IsNested || (mappingInfo?.IsNested(collectionField.Path) ?? false);

        return shouldUseNested
            ? new NestedQuery(collectionField.Path, inner)
            : inner;
    }

    /// <summary>
    /// 翻译 Enumerable.Contains
    /// </summary>
    private Query TranslateEnumerableContains(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        Expression collectionExpr;
        Expression valueExpr;

        if (node.Object != null)
        {
            // list.Contains(value)
            collectionExpr = node.Object;
            valueExpr = node.Arguments[0];
        }
        else
        {
            // Enumerable.Contains(list, value)
            collectionExpr = node.Arguments[0];
            valueExpr = node.Arguments[1];
        }

        var field = ResolveField(collectionExpr, prefix, mappingInfo);
        var value = Evaluate(valueExpr);

        return BuildTermsQuery(field, value!);
    }

    /// <summary>
    /// 翻译 Enumerable.All
    /// </summary>
    private Query TranslateEnumerableAll(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var collectionField = ResolveField(node.Arguments[0], prefix, mappingInfo);
        var predicate = UnwrapLambda(node.Arguments[1]);

        var inner = TranslateNode(predicate.Body, prefix: collectionField.Path, mappingInfo);

        return new NestedQuery(collectionField.Path, inner);
    }

    /// <summary>
    /// 翻译字符串方法
    /// </summary>
    private Query TranslateStringMethod(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var field = ResolveField(node.Object!, prefix, mappingInfo);
        var value = (string)Evaluate(node.Arguments[0])!;
        var fieldMapping = mappingInfo?.GetField(field.Path);
        var escapedValue = EscapeWildcard(value);

        return node.Method.Name switch
        {
            nameof(string.Contains) => TranslateContains(field, fieldMapping, value),
            nameof(string.StartsWith) => TranslateStartsWith(field, fieldMapping, value),
            nameof(string.EndsWith) => TranslateEndsWith(field, fieldMapping, value),
            _ => throw new NotSupportedException($"Unsupported string method {node.Method.Name}"),
        };
    }

    /// <summary>
    /// 翻译 Contains
    /// </summary>
    private Query TranslateContains(FieldInfo field, FieldMappingInfo? fieldMapping, string value)
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
        Expression fieldExpression;
        Expression valueExpression;

        if (node.Object != null)
        {
            fieldExpression = node.Object;
            valueExpression = node.Arguments[0];
        }
        else
        {
            fieldExpression = node.Arguments[0];
            valueExpression = node.Arguments[1];
        }

        var field = ResolveField(fieldExpression, prefix, mappingInfo);
        var value = Evaluate(valueExpression);

        if (value == null)
        {
            return new BoolQuery { MustNot = new Query[] { new ExistsQuery { Field = field.Path } } };
        }

        return BuildEquality(field, value);
    }

    /// <summary>
    /// 翻译 Enum.HasFlag
    /// </summary>
    private Query TranslateEnumHasFlag(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var field = ResolveField(node.Object!, prefix, mappingInfo);
        var flag = Evaluate(node.Arguments[0]);

        if (flag == null)
        {
            throw new NotSupportedException("Cannot use null flag in Enum.HasFlag");
        }

        var flagValue = Convert.ToInt64(flag);
        return new TermQuery { Field = field.Path, Value = flagValue };
    }

    /// <summary>
    /// 翻译集合 Contains
    /// </summary>
    private Query TranslateCollectionContains(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        // list.Contains(value) 或 hashSet.Contains(value)
        // node.Object = 集合实例 (可能是一个变量或常量)
        // node.Arguments[0] = value (要检查的值)

        // 尝试获取集合的值
        var collectionValue = Evaluate(node.Object!);

        // 如果集合是常量且可枚举，构建 TermsQuery
        if (collectionValue is IEnumerable enumerable && collectionValue is not string)
        {
            var values = enumerable.Cast<object>().Select(NormalizeValue).ToList();
            if (values.Count == 0)
            {
                return new MatchNoneQuery();
            }
            if (values.Count == 1)
            {
                // 如果集合只有一个值，使用 TermQuery
                return BuildEquality(ResolveField(node.Arguments[0], prefix, mappingInfo), values[0]);
            }
            // 多个值使用 TermsQuery
            var field = ResolveField(node.Arguments[0], prefix, mappingInfo);
            return new TermsQuery { Field = field.Path, Terms = new TermsQueryField(values) };
        }

        // 如果值是可枚举集合
        var value = Evaluate(node.Arguments[0]);
        if (value is IEnumerable enumerableValue && value is not string)
        {
            var values = enumerableValue.Cast<object>().Select(NormalizeValue).ToList();
            if (values.Count == 0)
            {
                return new MatchNoneQuery();
            }
            if (values.Count == 1)
            {
                return BuildEquality(ResolveField(node.Object!, prefix, mappingInfo), values[0]);
            }
            var field = ResolveField(node.Object!, prefix, mappingInfo);
            return new TermsQuery { Field = field.Path, Terms = new TermsQueryField(values) };
        }

        // 默认：检查字段是否在集合中
        // 这种情况下，我们使用 TermsQuery 但需要从外部获取集合值
        // 由于无法在编译时确定，使用 TermQuery 进行单值匹配
        var defaultField = ResolveField(node.Arguments[0], prefix, mappingInfo);
        return BuildEquality(defaultField, value!);
    }

    #endregion

    #region 查询构建

    /// <summary>
    /// 构建相等查询
    /// </summary>
    protected virtual Query BuildEquality(FieldInfo field, object value)
    {
        var fieldName = field.Path;

        // 1. Wildcard 字段 - 使用 Term 查询
        if (field.IsWildcard)
        {
            return new TermQuery
            {
                Field = fieldName,
                Value = value.ToString()!
            };
        }

        // 2. Keyword 字段或字符串
        if (field.IsKeyword || field.Type == typeof(string))
        {
            // 如果是枚举类型作为字符串存储,需要转换一下
            var stringValue = field.Type.IsEnum
                ? NormalizeEnumName(field.Type, value)
                : value.ToString()!;
            return new TermQuery { Field = fieldName, Value = stringValue };
        }

        // 3. 日期字段
        if (field.IsDate || field.Type == typeof(DateTime) || field.Type == typeof(DateTime?))
        {
            var date = (DateTime)value;
            return new DateRangeQuery
            {
                Field = fieldName,
                Gte = date,
                Lte = date,
                Format = field.Format ?? "yyyy-MM-dd HH:mm:ss||yyyy-MM-dd||epoch_millis"
            };
        }

        // 4. 数值字段
        if (field.IsNumeric || IsNumericType(field.Type))
        {
            return new TermQuery { Field = fieldName, Value = NormalizeValue(value) };
        }

        // 5. 布尔字段
        if (field.IsBoolean || field.Type == typeof(bool) || field.Type == typeof(bool?))
        {
            return new TermQuery { Field = fieldName, Value = (bool)value };
        }

        // 6. 枚举（值作为数值）
        if (field.Type.IsEnum)
        {
            return new TermQuery { Field = fieldName, Value = Convert.ToInt64(value) };
        }

        // 7. 如果值是枚举但字段类型不是枚举
        if (value.GetType().IsEnum)
        {
            return new TermQuery { Field = fieldName, Value = Convert.ToInt64(value) };
        }

        // 8. Text 字段 - 使用 MatchQuery
        if (field.IsText)
        {
            return new MatchQuery
            {
                Field = fieldName,
                Query = value.ToString()!
            };
        }

        // 9. 默认
        return new TermQuery { Field = fieldName, Value = NormalizeValue(value) };
    }

    /// <summary>
    /// 构建 NotEqual 查询
    /// </summary>
    private Query BuildNotEqualQuery(FieldInfo field, object value)
    {
        // 对于布尔值，直接取反值（更简洁）
        if (value is bool boolValue)
        {
            return new TermQuery { Field = field.Path, Value = !boolValue };
        }

        // 对于其他类型，使用 must_not 包装
        return new BoolQuery
        {
            MustNot = new Query[] { BuildEquality(field, value) }
        };
    }

    /// <summary>
    /// 构建 Terms 查询（用于集合包含）
    /// </summary>
    protected virtual Query BuildTermsQuery(FieldInfo field, object value)
    {
        var fieldName = field.Path;

        if (value is IEnumerable enumerable && value is not string)
        {
            var values = enumerable.Cast<object>().Select(NormalizeValue).ToList();
            if (values.Count == 0)
            {
                return new MatchNoneQuery();
            }
            if (values.Count == 1)
            {
                return BuildEquality(field, values[0]);
            }
            return new TermsQuery { Field = fieldName, Terms = new TermsQueryField(values) };
        }

        return BuildEquality(field, value);
    }

    /// <summary>
    /// 构建范围查询
    /// </summary>
    protected virtual Query BuildRange(
        FieldInfo field,
        object? greaterThan = null,
        object? greaterThanOrEqualTo = null,
        object? lessThan = null,
        object? lessThanOrEqualTo = null)
    {
        var fieldName = field.Path;

        // 日期范围
        if (field.IsDate || field.Type == typeof(DateTime) || field.Type == typeof(DateTime?))
        {
            var range = new DateRangeQuery
            {
                Field = fieldName,
                Format = field.Format ?? "yyyy-MM-dd HH:mm:ss||yyyy-MM-dd||epoch_millis"
            };

            if (greaterThan != null)
            {
                range.Gt = DateMath.FromString(NormalizeDate(greaterThan));
            }
            if (greaterThanOrEqualTo != null)
            {
                range.Gte = DateMath.FromString(NormalizeDate(greaterThanOrEqualTo));
            }
            if (lessThan != null)
            {
                range.Lt = DateMath.FromString(NormalizeDate(lessThan));
            }
            if (lessThanOrEqualTo != null)
            {
                range.Lte = DateMath.FromString(NormalizeDate(lessThanOrEqualTo));
            }

            return range;
        }

        // 数值范围
        var numberRange = new NumberRangeQuery { Field = fieldName };

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


    #endregion

    #region 辅助方法

    /// <summary>
    /// 解包 Lambda 表达式
    /// </summary>
    private static LambdaExpression UnwrapLambda(Expression expression)
    {
        while (expression is UnaryExpression { NodeType: ExpressionType.Quote } unary)
        {
            expression = unary.Operand;
        }
        return (LambdaExpression)expression;
    }

    /// <summary>
    /// 求值表达式
    /// </summary>
    private static object? Evaluate(Expression expression)
    {
        if (expression is ConstantExpression constant)
        {
            return constant.Value;
        }

        // 处理成员访问（捕获外部变量）
        if (expression is MemberExpression memberExpr)
        {
            object? obj = null;
            if (memberExpr.Expression != null)
            {
                obj = Evaluate(memberExpr.Expression);
            }

            if (memberExpr.Member is System.Reflection.FieldInfo fieldInfo)
            {
                return fieldInfo.GetValue(obj);
            }
            if (memberExpr.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo.GetValue(obj);
            }
        }

        // 处理数组索引
        if (expression is IndexExpression indexExpr)
        {
            var obj = Evaluate(indexExpr.Object!);
            var indices = indexExpr.Arguments.Select(Evaluate).ToArray();
            if (obj is Array array && indices.Length == 1 && indices[0] is int index)
            {
                return array.GetValue(index);
            }
        }

        // 处理方法调用
        if (expression is MethodCallExpression methodCall)
        {
            try
            {
                return Expression.Lambda(methodCall).Compile().DynamicInvoke();
            }
            catch
            {
                return null;
            }
        }

        // 处理 NewArrayExpression
        if (expression is NewArrayExpression newArray)
        {
            var values = newArray.Expressions.Select(Evaluate).ToArray();
            return values;
        }

        return Expression.Lambda(expression).Compile().DynamicInvoke();
    }

    /// <summary>
    /// 转义通配符
    /// </summary>
    private static string EscapeWildcard(string value)
    {
        return value.Replace("\\", "\\\\").Replace("*", "\\*").Replace("?", "\\?");
    }

    /// <summary>
    /// 标准化值
    /// </summary>
    private static FieldValue NormalizeValue(object value)
    {
        return value switch
        {
            string s => s,
            bool b => b,
            int i => i,
            long l => l,
            double d => d,
            float f => f,
            decimal dec => Convert.ToDouble(dec),
            Guid g => g.ToString(),
            Enum e => Convert.ToInt64(e),
            DateTime dt => NormalizeDate(dt),
            DateTimeOffset dto => NormalizeDate(dto),
            _ => Convert.ToDouble(value)
        };
    }

    /// <summary>
    /// 规范化日期值
    /// </summary>
    private static string NormalizeDate(object value)
    {
        if (value is DateTime dateTime)
        {
            // 格式化为 Elasticsearch 支持的格式（不带时区）
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");
        }

        return value.ToString()!;
    }
    /// <summary>
    /// 转换为 Number
    /// </summary>
    private static Number? ToNumber(object? value)
    {
        return value == null
            ? null
            : value switch
        {
            Number n => n,
            sbyte v => (Number)v,
            byte v => (Number)v,
            short v => (Number)v,
            ushort v => (Number)v,
            int v => (Number)v,
            uint v => (Number)v,
            long v => (Number)v,
            ulong v => (Number)v,
            float v => (Number)v,
            double v => (Number)v,
            decimal v => (Number)(double)v,
            _ => (Number)Convert.ToDouble(value)
        };
    }

    /// <summary>
    /// 规范化枚举名称
    /// </summary>
    private static string NormalizeEnumName(Type enumType, object value)
    {
        var enumValue = value is Enum e
            ? e
            : (Enum)Enum.ToObject(enumType, value);
        return enumValue.ToString();
    }

    /// <summary>
    /// 判断是否为数值类型
    /// </summary>
    private static bool IsNumericType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        return underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
               underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
               underlyingType == typeof(int) || underlyingType == typeof(uint) ||
               underlyingType == typeof(long) || underlyingType == typeof(ulong) ||
               underlyingType == typeof(float) || underlyingType == typeof(double) ||
               underlyingType == typeof(decimal);
    }

    /// <summary>
    /// 判断是否为 Nullable.HasValue 访问
    /// </summary>
    private static bool IsNullableHasValue(MemberExpression member)
    {
        return member.Member.Name == nameof(Nullable<>.HasValue)
               && member.Expression is MemberExpression
               && member.Member.DeclaringType?.IsGenericType == true
               && member.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// 判断是否为 Nullable.Value 访问
    /// </summary>
    private static bool IsNullableValueAccess(MemberExpression member)
    {
        return member.Member.Name == nameof(Nullable<>.Value)
               && member.Expression is MemberExpression
               && member.Member.DeclaringType?.IsGenericType == true
               && member.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    private static bool IsHasValueAccess(MemberExpression member)
    {
        return IsNullableHasValue(member);
    }

    #endregion

    #region 字段解析

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

    #endregion
}