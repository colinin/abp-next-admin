using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch;

/// <summary>
/// 表达式查询转换器 - 将 LINQ 表达式转换为 Elasticsearch Query
/// </summary>
public partial class ExpressionQueryTranslator : IExpressionQueryTranslator, ISingletonDependency
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

        var indexMapping = await _indexMappingProvider.GetMappingAsync<TDocument>(indexName);

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
        var methodCallQuery = TryTranslateMethodComparison(node, prefix, mappingInfo);
        if (methodCallQuery != null)
        {
            return methodCallQuery;
        }

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

        // 字符串方法
        if (node.Method.DeclaringType == typeof(string))
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
        // 如果是常量表达式，直接返回值
        if (expression is ConstantExpression constant)
        {
            return constant.Value;
        }

        // 如果是参数表达式，无法求值
        if (expression is ParameterExpression)
        {
            throw new InvalidOperationException($"Cannot evaluate parameter expression: {expression}");
        }

        // 处理成员访问（捕获外部变量）
        if (expression is MemberExpression memberExpr)
        {
            // 检查是否是字段或属性访问
            if (memberExpr.Member is System.Reflection.FieldInfo fieldInfo)
            {
                // 如果是静态字段
                if (fieldInfo.IsStatic)
                {
                    return fieldInfo.GetValue(null);
                }

                // 如果是实例字段，需要先求值实例
                if (memberExpr.Expression != null)
                {
                    var obj = Evaluate(memberExpr.Expression);
                    if (obj != null)
                    {
                        return fieldInfo.GetValue(obj);
                    }
                }

                // 如果无法求值，尝试编译整个表达式
                try
                {
                    return Expression.Lambda(memberExpr).Compile().DynamicInvoke();
                }
                catch
                {
                    throw new InvalidOperationException($"Cannot evaluate member expression: {memberExpr}");
                }
            }

            if (memberExpr.Member is PropertyInfo propertyInfo)
            {
                // 如果是静态属性
                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod != null && getMethod.IsStatic)
                {
                    return propertyInfo.GetValue(null);
                }

                // 如果是实例属性，需要先求值实例
                if (memberExpr.Expression != null)
                {
                    var obj = Evaluate(memberExpr.Expression);
                    if (obj != null)
                    {
                        return propertyInfo.GetValue(obj);
                    }
                }

                // 如果无法求值，尝试编译整个表达式
                try
                {
                    return Expression.Lambda(memberExpr).Compile().DynamicInvoke();
                }
                catch
                {
                    throw new InvalidOperationException($"Cannot evaluate member expression: {memberExpr}");
                }
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
                // 尝试编译并执行
                var lambda = Expression.Lambda(methodCall);
                return lambda.Compile().DynamicInvoke();
            }
            catch (InvalidOperationException)
            {
                // 如果包含参数引用，无法编译
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot evaluate method call: {methodCall}", ex);
            }
        }

        // 处理 NewArrayExpression
        if (expression is NewArrayExpression newArray)
        {
            var values = newArray.Expressions.Select(Evaluate).ToArray();
            return values;
        }

        // 默认尝试编译执行

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
}