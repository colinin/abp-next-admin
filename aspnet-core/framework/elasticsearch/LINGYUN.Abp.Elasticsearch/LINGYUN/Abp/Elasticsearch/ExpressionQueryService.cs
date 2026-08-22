using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch;

public class ExpressionQueryService : IExpressionQueryService, ITransientDependency
{
    public ILogger<ExpressionQueryService> Logger { protected get; set; }
    protected IElasticsearchClientFactory ClientFactory { get; }
    protected IIndexMappingProvider IndexMappingProvider { get; }
    protected IExpressionQueryTranslator ExpressionQueryTranslator { get; }

    public ExpressionQueryService(
        IElasticsearchClientFactory clientFactory,
        IIndexMappingProvider indexMappingProvider,
        IExpressionQueryTranslator expressionQueryTranslator)
    {
        ClientFactory = clientFactory;
        IndexMappingProvider = indexMappingProvider;
        ExpressionQueryTranslator = expressionQueryTranslator;

        Logger = NullLogger<ExpressionQueryService>.Instance;
    }

    public async virtual Task<long> GetCountAsync<TDocument>(
        string indexName,
        Expression<Func<TDocument, bool>> expression,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var client = ClientFactory.Create();
        var query = await ExpressionQueryTranslator.TranslateAsync(indexName, expression);

        var response = await client.CountAsync<TDocument>(dsl =>
            dsl.Indices(indexName).Query(query),
            cancellationToken);

        return response.Count;
    }

    public async virtual Task<List<TDocument>> GetListAsync<TDocument>(
        string indexName,
        Expression<Func<TDocument, bool>> expression,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Fields? sourceExcludes = null,
        Fields? sourceIncludes = null,
        object[]? beginMarker = null,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var client = ClientFactory.Create();
        var query = await ExpressionQueryTranslator.TranslateAsync(indexName, expression);

        SortOptions[]? sorts = null;
        if (!sorting.IsNullOrWhiteSpace())
        {
            var indexMapping = await IndexMappingProvider.GetMappingAsync<TDocument>(indexName, cancellationToken);
            if (indexMapping != null)
            {
                sorts = ResolveDefaultSorts(indexMapping, sorting);
            }
        }

        // 数量超过10000且存在排序时才可以使用SearchAfter特性
        return skipCount >= 10000 && sorts != null
            ? await SearchAfter<TDocument>(
                client, 
                indexName,
                query, 
                sorts, 
                maxResultCount,
                skipCount, 
                sourceExcludes, 
                sourceIncludes,
                beginMarker,
                cancellationToken)
            : await SearchFromSize<TDocument>(
                client,
                indexName,
                query,
                sorts,
                maxResultCount,
                skipCount,
                sourceExcludes,
                sourceIncludes,
                cancellationToken);
    }

    private async Task<List<TDocument>> SearchFromSize<TDocument>(
        ElasticsearchClient client,
        string indexName,
        Query query,
        SortOptions[]? sorts = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Fields? sourceExcludes = null,
        Fields? sourceIncludes = null,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<TDocument>(dsl =>
        {
            dsl.Indices(indexName)
                .Query(query)
               .From(skipCount)
               .Size(maxResultCount);
            if (sorts != null)
            {
                dsl.Sort(sorts);
            }
            if (sourceExcludes != null)
            {
                dsl.SourceExcludes(sourceExcludes);
            }
            if (sourceIncludes != null)
            {
                dsl.SourceIncludes(sourceIncludes);
            }
        }, cancellationToken);

        if (searchResponse.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query document failed: {errorMessage}", errorMessage);
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<TDocument>> SearchAfter<TDocument>(
        ElasticsearchClient client,
        string indexName,
        Query query,
        SortOptions[] sorts,
        int maxResultCount,
        int skipCount,
        Fields? sourceExcludes = null,
        Fields? sourceIncludes = null,
        object[]? beginMarker = null,
        CancellationToken cancellationToken = default)
    {
        List<FieldValue>? searchAfter = null;
        if (beginMarker != null)
        {
            searchAfter = beginMarker.Select(FieldValue.FromValue).ToList();
        }
        else
        {
            searchAfter = await GetSearchAfterValue(
                client,
                indexName,
                query,
                sorts,
                skipCount,
                cancellationToken);
        }

        if (searchAfter == null || !searchAfter.Any())
        {
            return [];
        }

        var searchResponse = await client.SearchAsync<TDocument>(dsl =>
        {
            dsl.Indices(indexName)
                .Query(query)
                .Sort(sorts)
                .Size(maxResultCount)
                .SearchAfter(searchAfter);
            if (sourceExcludes != null)
            {
                dsl.SourceExcludes(sourceExcludes);
            }
            if (sourceIncludes != null)
            {
                dsl.SourceIncludes(sourceIncludes);
            }
        }, cancellationToken);

        if (searchResponse.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query document failed: {errorMessage}", errorMessage);
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<FieldValue>?> GetSearchAfterValue(
        ElasticsearchClient client,
        string indexName,
        Query query,
        SortOptions[] sorts,
        int skipCount,
        CancellationToken cancellationToken = default)
    {
        // 10000以内直接取最后一条数据
        if (skipCount < 10000)
        {
            var response = await client.SearchAsync<EmptyDocument>(
                dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .From(skipCount)
                    .Size(1)
                    .Source(false)
                    .TrackScores(false),
                cancellationToken);

            if (response.TryGetErrorMessage(out var oneError))
            {
                Logger.LogWarning("Failed to obtain the {skipCount}th sorting record. error: {error}", skipCount, oneError);
                return null;
            }

            var hit = response.Hits.FirstOrDefault();
            return hit?.Sort?.ToList();
        }

        // 获取第9999条数据Hits作为searchAfter
        var firstResponse = await client.SearchAsync<EmptyDocument>(
            dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .From(9999)
                    .Size(1)
                    .Source(false)
                    .TrackScores(false),
            cancellationToken);

        if (firstResponse.TryGetErrorMessage(out var firstError))
        {
            Logger.LogWarning("Failed to obtain the first sorted record after the {skipCount}th item. error: {error}", skipCount, firstError);
            return null;
        }

        var firstHit = firstResponse.Hits?.FirstOrDefault();
        if (firstHit?.Sort == null || !firstHit.Sort.Any())
        {
            Logger.LogWarning("The first sorted record after the {skipCount}th item is empty!", skipCount);
            return null;
        }

        var remaining = skipCount - 10000;

        return await GetBatchSearchAfterValue(
            client,
            indexName,
            query,
            sorts,
            [.. firstHit.Sort],
            remaining,
            remaining > 10000 ? 5000 : 1000,
            cancellationToken);
    }

    private async Task<List<FieldValue>?> GetBatchSearchAfterValue(
        ElasticsearchClient client,
        string indexName,
        Query query,
        SortOptions[] sorts,
        FieldValue[] searchAfter,
        int remaining,
        int batchSize = 1000,
        CancellationToken cancellationToken = default)
    {
        List<FieldValue>? lastSort = null;

        while (remaining > 0)
        {
            var batch = Math.Min(remaining, batchSize);

            var response = await client.SearchAsync<EmptyDocument>(
                dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .SearchAfter(searchAfter)
                    .Size(batch)
                    .Source(false)
                    .TrackScores(false),
                cancellationToken);

            if (response.TryGetErrorMessage(out var error))
            {
                Logger.LogWarning("Failed to get batch records. remaining: {remaining}, error: {error}", remaining, error);
                return null;
            }

            if (response.Hits == null || !response.Hits.Any())
            {
                Logger.LogWarning("No more records available. remaining: {remaining}", remaining);
                return null;
            }

            var hits = response.Hits.ToList();
            var hitCount = hits.Count;

            remaining -= hitCount;

            if (remaining <= 0)
            {
                var targetIndex = hitCount + remaining;
                if (targetIndex == hitCount)
                {
                    lastSort = hits.LastOrDefault()?.Sort?.ToList();
                }
                else if (targetIndex >= 0 && targetIndex < hitCount)
                {
                    lastSort = hits[targetIndex]?.Sort?.ToList();
                }
                else
                {
                    return null;
                }

                return lastSort;
            }

            var lastHit = hits.LastOrDefault();
            if (lastHit?.Sort == null || !lastHit.Sort.Any())
            {
                return null;
            }

            searchAfter = [.. lastHit.Sort];

            if (hitCount < batch)
            {
                return null;
            }
        }

        return lastSort;
    }

    private static SortOptions[]? ResolveDefaultSorts(IndexMappingInfo indexMappingInfo, string? sorting = null)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            return null;
        }

        // eg: a desc, b.c asc; d+desc; e-asc; +f; -g
        var sortFields = sorting.Split([';', ','], StringSplitOptions.RemoveEmptyEntries);
        var sorts = new List<SortOptions>();

        foreach (var sortField in sortFields)
        {
            var trimmedSortField = sortField.Trim();
            if (trimmedSortField.IsNullOrWhiteSpace())
            {
                continue;
            }

            // [a, desc]
            // [b.c, asc]
            // [d, desc]
            // [e, asc]
            var parts = trimmedSortField.Split([' ', ':', '-', '+'], StringSplitOptions.RemoveEmptyEntries);

            string fieldName;
            SortOrder sortOrder;

            if (parts.Length >= 2)
            {
                // b.c
                fieldName = parts[0].Trim();
                // desc
                var orderStr = parts[1].Trim();
                sortOrder = orderStr.Equals("desc", StringComparison.InvariantCultureIgnoreCase) ||
                            orderStr.Equals("descending", StringComparison.InvariantCultureIgnoreCase)
                    ? SortOrder.Desc
                    : SortOrder.Asc;
            }
            else
            {
                fieldName = parts[0].Trim();
                // +f
                if (fieldName.StartsWith("+"))
                {
                    sortOrder = SortOrder.Asc;
                    fieldName = fieldName.Substring(1);
                }
                // -g
                else if (fieldName.StartsWith("-"))
                {
                    sortOrder = SortOrder.Desc;
                    fieldName = fieldName.Substring(1);
                }
                else
                {
                    sortOrder = SortOrder.Asc;
                }
            }

            var resolvedField = ResolveSortField(indexMappingInfo, fieldName);
            if (resolvedField != null)
            {
                var fieldPath = resolvedField.GetKeywordPath();
                if (!sorts.Any(x => x.Field?.Field?.Name == fieldPath))
                {
                    sorts.Add(new SortOptions
                    {
                        Field = new FieldSort(Field.FromString(fieldPath))
                        {
                            Order = sortOrder,
                        }
                    });
                }
            }
        }

        return sorts?.ToArray();
    }

    private static FieldMappingInfo? ResolveSortField(
        IndexMappingInfo indexMappingInfo,
        string fieldPath)
    {
        if (fieldPath.IsNullOrWhiteSpace())
        {
            return null;
        }

        var directField = indexMappingInfo.GetField(fieldPath);
        if (directField != null)
        {
            return directField;
        }

        var clrField = indexMappingInfo.GetFieldByClrPath(fieldPath);
        if (clrField != null)
        {
            return clrField;
        }

        var caseInsensitiveEsField = indexMappingInfo.Fields
            .FirstOrDefault(kvp => kvp.Key.Equals(fieldPath, StringComparison.InvariantCultureIgnoreCase))
            .Value;

        if (caseInsensitiveEsField != null)
        {
            return caseInsensitiveEsField;
        }

        var caseInsensitiveClrField = indexMappingInfo.ClrFields
            .FirstOrDefault(kvp => kvp.Key.Equals(fieldPath, StringComparison.InvariantCultureIgnoreCase))
            .Value;

        return caseInsensitiveClrField;
    }
}
