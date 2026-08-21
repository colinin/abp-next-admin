using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
    protected IElasticsearchClientFactory ClientFactory { get; }
    protected IExpressionQueryTranslator ExpressionQueryTranslator { get; }

    public ExpressionQueryService(
        IElasticsearchClientFactory clientFactory,
        IExpressionQueryTranslator expressionQueryTranslator)
    {
        ClientFactory = clientFactory;
        ExpressionQueryTranslator = expressionQueryTranslator;
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
            var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
                ? SortOrder.Asc : SortOrder.Desc;

            sorts = new SortOptions[1]
            {
                new SortOptions
                {
                    Field = new FieldSort(new Field(sorting))
                    {
                        Order = sortOrder,
                    },
                }
            };
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

        if (!searchResponse.IsSuccess())
        {
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
            searchAfter = await GetSearchAfterValue<TDocument>(
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

        if (!searchResponse.IsSuccess())
        {
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<FieldValue>?> GetSearchAfterValue<TDocument>(
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
            var response = await client.SearchAsync<TDocument>(
                dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .From(skipCount)
                    .Size(1),
                cancellationToken);

            if (!response.IsSuccess() || response.Hits == null || !response.Hits.Any())
            {
                return null;
            }

            var hit = response.Hits.FirstOrDefault();
            return hit?.Sort?.ToList();
        }

        // 获取第9999条数据Hits作为searchAfter
        var firstResponse = await client.SearchAsync<TDocument>(
            dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .SourceIncludes([])
                    .From(9999)
                    .Size(1),
            cancellationToken);

        if (!firstResponse.IsSuccess() || firstResponse.Hits == null || !firstResponse.Hits.Any())
        {
            return null;
        }

        var firstHit = firstResponse.Hits.FirstOrDefault();
        if (firstHit?.Sort == null || !firstHit.Sort.Any())
        {
            return null;
        }

        // 获取skipCount最近一条数据作为searchAfter
        var secondResponse = await client.SearchAsync<TDocument>(
            dsl => dsl.Indices(indexName)
                    .Query(query)
                    // 反转排序取第一个数据作为起始索引
                    .Sort(sorts.Select(x => x).Reverse().ToArray())
                    .SourceIncludes([])
                    .SearchAfter(firstHit.Sort.ToList())
                    .Size(1),
            cancellationToken);

        if (!secondResponse.IsSuccess() || secondResponse.Hits == null || !secondResponse.Hits.Any())
        {
            return null;
        }

        var lastHit = secondResponse.Hits.LastOrDefault();
        if (lastHit?.Sort == null || !lastHit.Sort.Any())
        {
            return null;
        }

        return lastHit.Sort.ToList();
    }
}
