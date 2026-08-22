using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.Elasticsearch;

public static class IndexMappingInfoExtensions
{
    public static string? GetElasticsearchFieldPath<TDocument>(
        this IndexMappingInfo mappingInfo,
        Expression<Func<TDocument, object?>> expression)
    {
        return mappingInfo.GetElasticsearchFieldPath(expression);
    }

    public static FieldMappingInfo? GetFieldMapping<TDocument>(
        this IndexMappingInfo mappingInfo,
        Expression<Func<TDocument, object?>> expression)
    {
        return mappingInfo.GetFieldByExpression(expression);
    }

    public static string? GetKeywordPath<TDocument>(
        this IndexMappingInfo mappingInfo,
        Expression<Func<TDocument, object?>> expression)
    {
        var field = mappingInfo.GetFieldByExpression(expression);
        return field?.GetKeywordPath();
    }
}
