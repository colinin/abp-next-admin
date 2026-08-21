using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchAuditLogManager : IAuditLogManager, ITransientDependency
{
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IIndexMappingProvider _indexMappingProvider;
    private readonly IExpressionQueryService _expressionQueryService;
    private readonly IClock _clock;

    public ILogger<ElasticsearchAuditLogManager> Logger { protected get; set; }

    public ElasticsearchAuditLogManager(
        IClock clock,
        IElasticsearchClientFactory clientFactory,
        IIndexNameNormalizer indexNameNormalizer,
        IIndexMappingProvider indexMappingProvider,
        IExpressionQueryService expressionQueryService)
    {
        _clock = clock;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _indexMappingProvider = indexMappingProvider;
        _expressionQueryService = expressionQueryService;

        Logger = NullLogger<ElasticsearchAuditLogManager>.Instance;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<AuditLog> specification,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();

        return await _expressionQueryService.GetCountAsync(
            indexName,
            specification.ToExpression(),
            cancellationToken);
    }

    public async virtual Task<List<AuditLog>> GetListAsync(
        ISpecification<AuditLog> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();

        var sortingField = sorting;
        if (sortingField.IsNullOrWhiteSpace())
        {
            var indexMapping = await _indexMappingProvider.GetMappingAsync(indexName, cancellationToken);
            if (indexMapping != null)
            {
                var sortingFieldMap = indexMapping.Fields
                    .Where(x => x.Key.Equals(sortingField, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Value)
                    .FirstOrDefault();
                if (sortingFieldMap != null)
                {
                    sortingField = sortingFieldMap.Path;
                }
            }
        }

        return await _expressionQueryService.GetListAsync(
            indexName,
            specification.ToExpression(),
            sortingField,
            maxResultCount,
            skipCount,
            sourceExcludes: includeDetails == true
                ? Fields.FromFields(
                [
                    new Field("Actions"),
                    new Field("Comments"),
                    new Field("EntityChanges"),
                    new Field("Exceptions"),
                ])
                : null,
            cancellationToken: cancellationToken);
    }

    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? httpMethod = null,
        string? url = null,
        Guid? userId = null,
        string? userName = null,
        string? applicationName = null,
        string? correlationId = null,
        string? clientId = null,
        string? clientIpAddress = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();
        var client = _clientFactory.Create();
        var indexMapping = await _indexMappingProvider.GetMappingAsync(indexName, cancellationToken);

        var querys = BuildQueryDescriptor(
            indexMapping,
            startTime,
            endTime,
            httpMethod,
            url,
            userId,
            userName,
            applicationName,
            correlationId,
            clientId,
            clientIpAddress,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode);

        var response = await client.CountAsync<AuditLog>(dsl =>
            dsl.Indices(indexName)
               .Query(new BoolQuery
               {
                   Must = querys
               }),
            cancellationToken);

        if (response.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query audit log count failed: {errorMessage}", errorMessage);
        }

        return response.Count;
    }

    public async virtual Task<List<AuditLog>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? httpMethod = null,
        string? url = null,
        Guid? userId = null,
        string? userName = null,
        string? applicationName = null,
        string? correlationId = null,
        string? clientId = null,
        string? clientIpAddress = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();
        var client = _clientFactory.Create();
        var indexMapping = await _indexMappingProvider.GetMappingAsync(indexName, cancellationToken);

        var sorts = GetOrDefaultSort(indexMapping, sorting);

        var querys = BuildQueryDescriptor(
            indexMapping,
            startTime,
            endTime,
            httpMethod,
            url,
            userId,
            userName,
            applicationName,
            correlationId,
            clientId,
            clientIpAddress,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode);

        var query = new BoolQuery { Must = querys };

        // ES最大支持10000, 超出这个长度后升级为使用Search_After方案
        return skipCount >= 10000 && sorts != null
            ? await SearchAfterAuditLogs(
                client, 
                indexName,
                query,
                sorts,
                maxResultCount,
                skipCount,
                includeDetails,
                cancellationToken)
            : await SearchFromSizeAuditLogs(
                client,
                indexName,
                query,
                sorts,
                maxResultCount,
                skipCount,
                includeDetails,
                cancellationToken);
    }

    public async virtual Task<AuditLog?> GetAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var response = await client.GetAsync<AuditLog>(
            id.ToString(),
            dsl =>
            {
                dsl.Index(CreateIndex());
                if (!includeDetails)
                {
                    dsl.SourceExcludes(
                        ex => ex.Actions,
                        ex => ex.Comments,
                        ex => ex.Exceptions,
                        ex => ex.EntityChanges);
                }
            },
            cancellationToken);

        return response.Source;
    }

    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        await client.DeleteAsync<AuditLog>(
            id.ToString(),
            dsl => dsl.Index(CreateIndex()),
            cancellationToken);
    }

    public async virtual Task DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var idValues = ids.Select(id => FieldValue.String(id.ToString())).ToList();
        await client.DeleteByQueryAsync<AuditLog>(
            x => x.Indices(CreateIndex())
                  .Query(query =>
                    query.Terms(terms =>
                        terms.Field(field => field.Id)
                            .Terms(new TermsQueryField(idValues)))),
            cancellationToken);
    }

    protected virtual List<Query> BuildQueryDescriptor(
        IndexMappingInfo indexMappingInfo,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? httpMethod = null,
        string? url = null,
        Guid? userId = null,
        string? userName = null,
        string? applicationName = null,
        string? correlationId = null,
        string? clientId = null,
        string? clientIpAddress = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null)
    {
        var queries = new List<Query>();

        if (startTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(indexMappingInfo, nameof(AuditLog.ExecutionTime)))
            {
                Gte = _clock.Normalize(startTime.Value)
            });
        }
        if (endTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(indexMappingInfo, nameof(AuditLog.ExecutionTime)))
            {
                Lte = _clock.Normalize(endTime.Value)
            });
        }
        if (!httpMethod.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.HttpMethod)), httpMethod));
        }
        if (!url.IsNullOrWhiteSpace())
        {
            queries.Add(new WildcardQuery(GetField(indexMappingInfo, nameof(AuditLog.Url)))
            {
                Value = $"*{url}*"
            });
        }
        if (userId.HasValue)
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.UserId)), userId.Value.ToString()));
        }
        if (!userName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.UserName)), userName));
        }
        if (!applicationName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.ApplicationName)), applicationName));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.CorrelationId)), correlationId));
        }
        if (!clientId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.ClientId)), clientId));
        }
        if (!clientIpAddress.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.ClientIpAddress)), clientIpAddress));
        }
        if (maxExecutionDuration.HasValue)
        {
            queries.Add(new NumberRangeQuery(GetField(indexMappingInfo, nameof(AuditLog.ExecutionDuration)))
            {
                Lte = maxExecutionDuration.Value
            });
        }
        if (minExecutionDuration.HasValue)
        {
            queries.Add(new NumberRangeQuery(GetField(indexMappingInfo, nameof(AuditLog.ExecutionDuration)))
            {
                Gte = minExecutionDuration.Value
            });
        }


        if (hasException.HasValue)
        {
            if (hasException.Value)
            {
                queries.Add(new ExistsQuery(GetField(indexMappingInfo, nameof(AuditLog.Exceptions))));
            }
            else
            {
                queries.Add(new BoolQuery
                {
                    MustNot = new List<Query>
                    {
                        new ExistsQuery(GetField(indexMappingInfo, nameof(AuditLog.Exceptions)))
                    }
                });
            }
        }

        if (httpStatusCode.HasValue)
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, nameof(AuditLog.HttpStatusCode)), ((int)httpStatusCode.Value).ToString()));
        }

        return queries;
    }

    private async Task<List<AuditLog>> SearchFromSizeAuditLogs(
        ElasticsearchClient client,
        string indexName,
        Query query,
        SortOptions[]? sorts = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(indexName)
                .Query(query)
               .From(skipCount)
               .Size(maxResultCount);
            if (sorts != null)
            {
                dsl.Sort(sorts);
            }

            if (!includeDetails)
            {
                dsl.SourceExcludes(
                    ex => ex.Actions,
                    ex => ex.Comments,
                    ex => ex.Exceptions,
                    ex => ex.EntityChanges);
            }
        }, cancellationToken);

        if (searchResponse.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query audit log failed: {errorMessage}", errorMessage);
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<AuditLog>> SearchAfterAuditLogs(
        ElasticsearchClient client,
        string indexName,
        Query query,
        SortOptions[] sorts,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var searchAfter = await GetSearchAfterValue(
            client,
            indexName,
            query,
            sorts,
            skipCount,
            cancellationToken);

        if (searchAfter == null || !searchAfter.Any())
        {
            return [];
        }

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(indexName)
                .Query(query)
                .Sort(sorts)
                .Size(maxResultCount)
                .SearchAfter(searchAfter);

            if (!includeDetails)
            {
                dsl.SourceExcludes(
                    ex => ex.Actions,
                    ex => ex.Comments,
                    ex => ex.Exceptions,
                    ex => ex.EntityChanges);
            }
        }, cancellationToken);

        if (searchResponse.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query audit log failed: {errorMessage}", errorMessage);
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
            var response = await client.SearchAsync<AuditLog>(
                dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .SourceIncludes(x => x.Id)
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
        var firstResponse = await client.SearchAsync<AuditLog>(
            dsl => dsl.Indices(indexName)
                    .Query(query)
                    .Sort(sorts)
                    .SourceIncludes(x => x.Id)
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
        var secondResponse = await client.SearchAsync<AuditLog>(
            dsl => dsl.Indices(indexName)
                    .Query(query)
                    // 反转排序取第一个数据作为起始索引
                    .Sort(sorts.Select(x => x).Reverse().ToArray())
                    .SourceIncludes(x => x.Id)
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

    protected virtual string CreateIndex()
    {
        return _indexNameNormalizer.NormalizeIndex("audit-log");
    }

    private static SortOptions[]? GetOrDefaultSort(IndexMappingInfo indexMappingInfo, string? sorting = null)
    {
        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
            ? SortOrder.Asc : SortOrder.Desc;
        sorting = !sorting.IsNullOrWhiteSpace()
            ? sorting.Split()[0]
            : nameof(AuditLog.ExecutionTime);

        SortOptions[]? sorts = null;
        var sortingFieldMap = indexMappingInfo.Fields
                    .Where(x => x.Key.Equals(sorting, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Value)
                    .FirstOrDefault();
        if (sortingFieldMap != null)
        {
            sorting = sortingFieldMap.Path;
        }
        if (!sorting.IsNullOrWhiteSpace())
        {
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

        return sorts;
    }

    private static string GetField(IndexMappingInfo indexMappingInfo, string fieldFullPath)
    {
        return indexMappingInfo.GetExactFieldPath(fieldFullPath);
    }
}
