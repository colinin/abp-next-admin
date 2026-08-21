using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace LINGYUN.Abp.Elasticsearch;

public partial class ExpressionQueryTranslator
{
    /// <summary>
    /// 翻译集合 Contains
    /// </summary>
    private Query TranslateCollectionContains(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        // list.Contains(value) 或 hashSet.Contains(value)
        // 有两种情况：
        // 1. 外部集合.Contains(字段) - 例如：new[]{"a","b"}.Contains(x.xxx)
        // 2. 字段.Contains(值) - 例如：x.Y.Contains("B")

        Expression collectionExpr = node.Object!;
        Expression valueExpr = node.Arguments[0];

        // 判断哪个是字段，哪个是值
        var collectionIsField = IsFieldLike(collectionExpr);
        var valueIsField = IsFieldLike(valueExpr);

        if (collectionIsField && !valueIsField)
        {
            // 情况 2：字段.Contains(值)
            // 这通常用于集合字段，如 x.Tags.Contains("B")
            var field = ResolveField(collectionExpr, prefix, mappingInfo);
            var value = Evaluate(valueExpr);

            if (value == null)
            {
                return new MatchNoneQuery();
            }

            // 对于集合字段，使用 Terms 查询或 Term 查询
            // 如果字段是数组或集合类型，单个值的 Contains 实际上就是 Term 查询
            return BuildEquality(field, value);
        }
        else if (!collectionIsField && valueIsField)
        {
            // 情况 1：外部集合.Contains(字段)
            var field = ResolveField(valueExpr, prefix, mappingInfo);

            // 尝试获取集合的值
            var collectionValue = Evaluate(collectionExpr);

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
                    return BuildEquality(field, values[0]);
                }
                // 多个值使用 TermsQuery
                return new TermsQuery { Field = field.Path, Terms = new TermsQueryField(values) };
            }

            // 如果集合值无法获取，使用默认处理
            var defaultValue = Evaluate(valueExpr);
            return BuildEquality(field, defaultValue!);
        }
        else if (collectionIsField && valueIsField)
        {
            // 两个都是字段，这种情况较少见
            throw new NotSupportedException($"Unsupported Contains with two field expressions: {node}");
        }
        else
        {
            // 两个都不是字段，尝试直接求值
            var collectionValue = Evaluate(collectionExpr);
            var value = Evaluate(valueExpr);

            if (collectionValue is IEnumerable enumerable && collectionValue is not string)
            {
                if (enumerable.Cast<object>().Contains(value))
                {
                    return new MatchAllQuery();
                }
                else
                {
                    return new MatchNoneQuery();
                }
            }

            throw new NotSupportedException($"Unsupported Contains expression: {node}");
        }
    }
}
