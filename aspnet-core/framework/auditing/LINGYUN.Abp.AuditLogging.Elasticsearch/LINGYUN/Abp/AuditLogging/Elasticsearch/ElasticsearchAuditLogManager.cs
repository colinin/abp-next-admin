using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
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
    private readonly AbpElasticsearchOptions _elasticsearchOptions;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IClock _clock;

    public ILogger<ElasticsearchAuditLogManager> Logger { protected get; set; }

    public ElasticsearchAuditLogManager(
        IClock clock,
        IIndexNameNormalizer indexNameNormalizer,
        IOptions<AbpElasticsearchOptions> elasticsearchOptions,
        IElasticsearchClientFactory clientFactory)
    {
        _clock = clock;
        _clientFactory = clientFactory;
        _elasticsearchOptions = elasticsearchOptions.Value;
        _indexNameNormalizer = indexNameNormalizer;

        Logger = NullLogger<ElasticsearchAuditLogManager>.Instance;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<AuditLog> specification,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();
        var actionsIsNested = await GetActionsIsNested(client, cancellationToken);
        var translator = new AuditLogExpressionQueryTranslator(actionsIsNested);
        var query = translator.Translate(specification.ToExpression());

        var response = await client.CountAsync<AuditLog>(dsl =>
            dsl.Indices(CreateIndex()).Query(query),
            cancellationToken);

        return response.Count;
    }

    public async virtual Task<List<AuditLog>> GetListAsync(
        ISpecification<AuditLog> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();
        var actionsIsNested = await GetActionsIsNested(client, cancellationToken);
        var translator = new AuditLogExpressionQueryTranslator(actionsIsNested);
        var query = translator.Translate(specification.ToExpression());

        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
            ? SortOrder.Asc : SortOrder.Desc;
        sorting = !sorting.IsNullOrWhiteSpace()
            ? sorting.Split()[0]
            : nameof(AuditLog.ExecutionTime);
        // ES最大支持10000, 超出这个长度后升级为使用Search_After方案

        return skipCount >= 10000
            ? await SearchAfterAuditLogs(client, query, sorting, sortOrder, maxResultCount, skipCount, includeDetails, cancellationToken)
            : await SearchFromSizeAuditLogs(client, query, sorting, sortOrder, maxResultCount, skipCount, includeDetails, cancellationToken);
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
        var client = _clientFactory.Create();

        var querys = BuildQueryDescriptor(
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
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               }),
            cancellationToken);

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
        var client = _clientFactory.Create();

        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
            ? SortOrder.Asc : SortOrder.Desc;
        sorting = !sorting.IsNullOrWhiteSpace()
            ? sorting.Split()[0]
            : nameof(AuditLog.ExecutionTime);

        var querys = BuildQueryDescriptor(
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
        return skipCount >= 10000
            ? await SearchAfterAuditLogs(client, query, sorting, sortOrder, maxResultCount, skipCount, includeDetails, cancellationToken)
            : await SearchFromSizeAuditLogs(client, query, sorting, sortOrder, maxResultCount, skipCount, includeDetails, cancellationToken);
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
            queries.Add(new DateRangeQuery(GetField(nameof(AuditLog.ExecutionTime)))
            {
                Gte = _clock.Normalize(startTime.Value)
            });
        }
        if (endTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(AuditLog.ExecutionTime)))
            {
                Lte = _clock.Normalize(endTime.Value)
            });
        }
        if (!httpMethod.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.HttpMethod)), httpMethod));
        }
        if (!url.IsNullOrWhiteSpace())
        {
            queries.Add(new WildcardQuery(GetField(nameof(AuditLog.Url)))
            {
                Value = $"*{url}*"
            });
        }
        if (userId.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.UserId)), userId.Value.ToString()));
        }
        if (!userName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.UserName)), userName));
        }
        if (!applicationName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.ApplicationName)), applicationName));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.CorrelationId)), correlationId));
        }
        if (!clientId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.ClientId)), clientId));
        }
        if (!clientIpAddress.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.ClientIpAddress)), clientIpAddress));
        }
        if (maxExecutionDuration.HasValue)
        {
            queries.Add(new NumberRangeQuery(GetField(nameof(AuditLog.ExecutionDuration)))
            {
                Lte = maxExecutionDuration.Value
            });
        }
        if (minExecutionDuration.HasValue)
        {
            queries.Add(new NumberRangeQuery(GetField(nameof(AuditLog.ExecutionDuration)))
            {
                Gte = minExecutionDuration.Value
            });
        }


        if (hasException.HasValue)
        {
            if (hasException.Value)
            {
                queries.Add(new ExistsQuery(GetField("Exceptions")));
            }
            else
            {
                queries.Add(new BoolQuery
                {
                    MustNot = new List<Query>
                    {
                        new ExistsQuery(GetField("Exceptions"))
                    }
                });
            }
        }

        if (httpStatusCode.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(AuditLog.HttpStatusCode)), ((int)httpStatusCode.Value).ToString()));
        }

        return queries;
    }

    private async Task<bool> GetActionsIsNested(ElasticsearchClient client, CancellationToken cancellationToken = default)
    {
        var actionsIsNested = false;

        var response = await client.Indices.GetMappingAsync<AuditLog>(
            d => d.Indices(CreateIndex()),
            cancellationToken);

        foreach (var mapping in response.Mappings)
        {
            if (mapping.Value.Mappings?.Properties is IDictionary<PropertyName, IProperty> properties &&
                properties.TryGetValue("Actions", out var actionsProperty))
            {
                actionsIsNested = actionsProperty is NestedProperty;
                break;
            }
        }

        return actionsIsNested;
    }

    private async Task<List<AuditLog>> SearchFromSizeAuditLogs(
        ElasticsearchClient client,
        Query query,
        string sorting,
        SortOrder sortOrder,
        int maxResultCount,
        int skipCount,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
                .Query(query)
                .Sort(s => s.Field(new FieldSort(GetField(sorting))
                {
                    Order = sortOrder
                }))
               .From(skipCount)
               .Size(maxResultCount);

            if (!includeDetails)
            {
                dsl.SourceExcludes(
                    ex => ex.Actions,
                    ex => ex.Comments,
                    ex => ex.Exceptions,
                    ex => ex.EntityChanges);
            }
        }, cancellationToken);

        if (!searchResponse.IsSuccess())
        {
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<AuditLog>> SearchAfterAuditLogs(
        ElasticsearchClient client,
        Query query,
        string sorting,
        SortOrder sortOrder,
        int maxResultCount,
        int skipCount,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var searchAfter = await GetSearchAfterValue(
            client,
            query,
            sorting,
            sortOrder,
            skipCount,
            cancellationToken);

        if (searchAfter == null || !searchAfter.Any())
        {
            return [];
        }

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
                .Query(query)
                .Sort(s => s.Field(new FieldSort(GetField(sorting))
                {
                    Order = sortOrder
                }))
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

        if (!searchResponse.IsSuccess())
        {
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<FieldValue>?> GetSearchAfterValue(
        ElasticsearchClient client,
        Query query,
        string sorting,
        SortOrder sortOrder,
        int skipCount,
        CancellationToken cancellationToken = default)
    {
        // 10000以内直接取最后一条数据
        if (skipCount < 10000)
        {
            var response = await client.SearchAsync<AuditLog>(
                dsl => dsl.Indices(CreateIndex())
                    .Query(query)
                    .Sort(s => s.Field(new FieldSort(GetField(sorting))
                    {
                        Order = sortOrder
                    }))
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
            dsl => dsl.Indices(CreateIndex())
                    .Query(query)
                    .Sort(s => s.Field(new FieldSort(GetField(sorting))
                    {
                        Order = sortOrder
                    }))
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

        var remaining = skipCount - 10000;
        // 获取skipCount最近一条数据作为searchAfter
        var secondResponse = await client.SearchAsync<AuditLog>(
            dsl => dsl.Indices(CreateIndex())
                    .Query(query)
                    .Sort(s => s.Field(new FieldSort(GetField(sorting))
                    {
                        Order = sortOrder
                    }))
                    .SourceIncludes(x => x.Id)
                    .SearchAfter(firstHit.Sort.ToList())
                    .Size(1),
            cancellationToken);

        if (!secondResponse.IsSuccess() || secondResponse.Hits == null || !secondResponse.Hits.Any())
        {
            return null;
        }

        if (secondResponse.Hits.Count < remaining)
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

    protected virtual string GetField(string field)
    {
        if (_auditLogFieldMaps.TryGetValue(field, out var mapField))
        {
            return _elasticsearchOptions.FieldCamelCase ? mapField.ToCamelCase() : mapField.ToPascalCase();
        }

        return _elasticsearchOptions.FieldCamelCase ? field.ToCamelCase() : field.ToPascalCase();
    }

    private readonly static IDictionary<string, string> _auditLogFieldMaps = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
    {
        { "Id", "Id.keyword" },
        { "ApplicationName", "ApplicationName.keyword" },
        { "UserId", "UserId.keyword" },
        { "UserName", "UserName.keyword" },
        { "TenantId", "TenantId.keyword" },
        { "TenantName", "TenantName.keyword" },
        { "ImpersonatorUserId", "ImpersonatorUserId.keyword" },
        { "ImpersonatorTenantId", "ImpersonatorTenantId.keyword" },
        { "ClientName", "ClientName.keyword" },
        { "ClientIpAddress", "ClientIpAddress.keyword" },
        { "ClientId", "ClientId.keyword" },
        { "CorrelationId", "CorrelationId.keyword" },
        { "BrowserInfo", "BrowserInfo.keyword" },
        { "HttpMethod", "HttpMethod.keyword" },
        { "Url", "Url.keyword" },
        { "ExecutionDuration", "ExecutionDuration" },
        { "ExecutionTime", "ExecutionTime" },
        { "HttpStatusCode", "HttpStatusCode" },
    };
}
