using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.Elasticsearch;

public partial class ExpressionQueryTranslator
{
    /// <summary>
    /// 尝试翻译包含方法调用的比较表达式
    /// </summary>
    private Query? TryTranslateMethodComparison(BinaryExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        // 处理 (field.Method(value) == 0) 或 (field.Method(value) >= 0) 这类表达式
        if (node.Left is MethodCallExpression leftMethodCall)
        {
            return TranslateMethodComparison(leftMethodCall, node.Right, node.NodeType, prefix, mappingInfo);
        }

        // 处理 (0 == field.Method(value)) 或 (0 <= field.Method(value)) 这类表达式
        if (node.Right is MethodCallExpression rightMethodCall)
        {
            // 反转比较操作符
            var reversedNodeType = ReverseComparisonOperator(node.NodeType);
            return TranslateMethodComparison(rightMethodCall, node.Left, reversedNodeType, prefix, mappingInfo);
        }

        return null;
    }

    /// <summary>
    /// 翻译方法调用比较
    /// </summary>
    private Query? TranslateMethodComparison(
        MethodCallExpression methodCall,
        Expression constantExpression,
        ExpressionType comparisonType,
        string? prefix,
        IndexMappingInfo? mappingInfo)
    {
        // 限定string方法调用
        if (methodCall.Method.DeclaringType != typeof(string))
        {
            return methodCall.Method.Name switch
            {
                nameof(string.CompareTo) => TranslateStringCompareToComparison(methodCall, constantExpression, comparisonType, prefix, mappingInfo),
                nameof(string.IndexOf) => TranslateStringIndexOfComparison(methodCall, constantExpression, comparisonType, prefix, mappingInfo),
                _ => null,
            };
        }
        return null;
    }

    /// <summary>
    /// 反转比较操作符
    /// </summary>
    private ExpressionType ReverseComparisonOperator(ExpressionType nodeType)
    {
        return nodeType switch
        {
            ExpressionType.GreaterThan => ExpressionType.LessThan,
            ExpressionType.GreaterThanOrEqual => ExpressionType.LessThanOrEqual,
            ExpressionType.LessThan => ExpressionType.GreaterThan,
            ExpressionType.LessThanOrEqual => ExpressionType.GreaterThanOrEqual,
            _ => nodeType
        };
    }
}
