using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Linq.Dynamic.Queryable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class SerilogElasticsearchLoggingManager : ILoggingManager, ISingletonDependency
{
    private readonly static Regex _indexFormatRegex = new Regex(@"^(.*)(?:\{0\:.+\})(.*)$");
    private readonly static Dictionary<Type, Type> _defaultTypeMap = new Dictionary<Type, Type>
    {
        [typeof(LogInfo)] = typeof(SerilogInfo),
        [typeof(LogLevel)] = typeof(string),
        [typeof(LogField)] = typeof(SerilogField),
        [typeof(LogException)] = typeof(SerilogException),
    };

    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly AbpLoggingSerilogElasticsearchOptions _options;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IIndexMappingProvider _indexMappingProvider;
    private readonly IExpressionQueryService _expressionQueryService;
    private readonly IObjectMapper<AbpLoggingSerilogElasticsearchModule> _objectMapper;

    public ILogger<SerilogElasticsearchLoggingManager> Logger { protected get; set; }

    public SerilogElasticsearchLoggingManager(
        IClock clock,
        ICurrentTenant currentTenant,
        IOptions<AbpLoggingSerilogElasticsearchOptions> options,
        IElasticsearchClientFactory clientFactory,
        IIndexMappingProvider indexMappingProvider,
        IExpressionQueryService expressionQueryService,
        IObjectMapper<AbpLoggingSerilogElasticsearchModule> objectMapper)
    {
        _clock = clock;
        _objectMapper = objectMapper;
        _currentTenant = currentTenant;
        _clientFactory = clientFactory;
        _indexMappingProvider = indexMappingProvider;
        _expressionQueryService = expressionQueryService;
        _options = options.Value;

        Logger = NullLogger<SerilogElasticsearchLoggingManager>.Instance;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<LogInfo> specification,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();
        var converter = new ExpressionQueryConverter<LogInfo, SerilogInfo>(_defaultTypeMap);
        var expression = converter.Convert(specification.ToExpression());

        return await _expressionQueryService.GetCountAsync(
            indexName, 
            expression,
            cancellationToken);
    }

    public async virtual Task<List<LogInfo>> GetListAsync(
        ISpecification<LogInfo> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();
        var converter = new ExpressionQueryConverter<LogInfo, SerilogInfo>(_defaultTypeMap);
        var expression = converter.Convert(specification.ToExpression());

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

        var serilogLogs = await _expressionQueryService.GetListAsync(
            indexName,
            expression,
            sortingField,
            maxResultCount,
            skipCount,
            cancellationToken: cancellationToken);

        return _objectMapper.Map<List<SerilogInfo>, List<LogInfo>>(serilogLogs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">时间类型或者转换为timestamp都可以查询</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async virtual Task<LogInfo?> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();
        var client = _clientFactory.Create();
        var indexMapping = await _indexMappingProvider.GetMappingAsync(indexName, cancellationToken);

        SearchResponse<SerilogInfo> response;

        if (_currentTenant.IsAvailable)
        {
            /*
            "query": {
                "bool": {
                    "must": [
                        {
                            "term": {
                                "fields.TenantId.keyword": {
                                    "value": _currentTenant.GetId()
                                }
                            }
                        },
                        {
                            "term": {
                                "fields.UniqueId": {
                                    "value": "1474021081433481216"
                                }
                            }
                        }
                    ]
                }
            }
            */
            response = await client.SearchAsync<SerilogInfo>(
                dsl =>
                    dsl.Indices(CreateIndex())
                       .Query(
                            (q) => q.Bool(
                                (b) => b.Must(
                                    (s) => s.Term(
                                        (t) => t.Field(GetField(indexMapping, "fields.UniqueId")).Value(id)),
                                    (s) => s.Term(
                                        (t) => t.Field(GetField(indexMapping, "fields.TenantId")).Value(_currentTenant.GetId().ToString())))))
                       .Size(1),
                cancellationToken);
        }
        else
        {
            /*
            "query": {
                "bool": {
                    "must": [
                        {
                            "term": {
                                "fields.UniqueId": {
                                    "value": "1474021081433481216"
                                }
                            }
                        }
                    ]
                }
            }
            */
            response = await client.SearchAsync<SerilogInfo>(
                dsl =>
                    dsl.Indices(CreateIndex())
                       .Query(
                            (q) => q.Bool(
                                (b) => b.Must(
                                    (s) => s.Term(
                                        (t) => t.Field(GetField(indexMapping, "fields.UniqueId")).Value(id)))))
                       .Size(1),
                cancellationToken);
            if (response.TryGetErrorMessage(out var errorMessage))
            {
                Logger.LogWarning("Query logs failed: {errorMessage}", errorMessage);
            }
        }

        return _objectMapper.Map<SerilogInfo?, LogInfo?>(response.Documents.FirstOrDefault());
    }

    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string? machineName = null,
        string? environment = null,
        string? application = null,
        string? context = null,
        string? requestId = null,
        string? requestPath = null,
        string? correlationId = null,
        int? processId = null,
        int? threadId = null,
        bool? hasException = null,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndex();
        var client = _clientFactory.Create();
        var indexMapping = await _indexMappingProvider.GetMappingAsync(indexName, cancellationToken);

        var querys = BuildQueryDescriptor(
            indexMapping,
            startTime,
            endTime,
            level,
            machineName,
            environment,
            application,
            context,
            requestId,
            requestPath,
            correlationId,
            processId,
            threadId,
            hasException);

        var response = await client.CountAsync<SerilogInfo>((dsl) =>
            dsl.Indices(indexName)
               .Query(log => log.Bool(b => b.Must(querys.ToArray()))),
            cancellationToken);
        if (response.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query log count failed: {errorMessage}", errorMessage);
        }

        return response.Count;
    }

    /// <summary>
    /// 获取日志列表
    /// </summary>
    /// <param name="sorting">排序字段</param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="level"></param>
    /// <param name="machineName"></param>
    /// <param name="environment"></param>
    /// <param name="application"></param>
    /// <param name="context"></param>
    /// <param name="requestId"></param>
    /// <param name="requestPath"></param>
    /// <param name="correlationId"></param>
    /// <param name="processId"></param>
    /// <param name="threadId"></param>
    /// <param name="hasException"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async virtual Task<List<LogInfo>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string? machineName = null,
        string? environment = null,
        string? application = null,
        string? context = null,
        string? requestId = null,
        string? requestPath = null,
        string? correlationId = null,
        int? processId = null,
        int? threadId = null,
        bool? hasException = null,
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
            level,
            machineName,
            environment,
            application,
            context,
            requestId,
            requestPath,
            correlationId,
            processId,
            threadId,
            hasException);

        var query = new BoolQuery { Must = querys };

        var serilogLogs = skipCount >= 10000 && sorts != null
            ? await SearchAfterSerilogLogs(client, query, sorts, maxResultCount, skipCount, cancellationToken)
            : await SearchFromSizeSerilogLogs(client, query, sorts, maxResultCount, skipCount, cancellationToken);

        return _objectMapper.Map<List<SerilogInfo>, List<LogInfo>>(serilogLogs);
    }

    protected virtual List<Query> BuildQueryDescriptor(
        IndexMappingInfo indexMappingInfo,
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string? machineName = null,
        string? environment = null,
        string? application = null,
        string? context = null,
        string? requestId = null,
        string? requestPath = null,
        string? correlationId = null,
        int? processId = null,
        int? threadId = null,
        bool? hasException = null)
    {
        var queries = new List<Query>();

        if (_currentTenant.IsAvailable)
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, "fields.TenantId"), _currentTenant.GetId().ToString()));
        }
        if (startTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(indexMappingInfo, "@timestamp"))
            {
                Gte = _clock.Normalize(startTime.Value),
            });
        }
        if (endTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(indexMappingInfo, "@timestamp"))
            {
                Lte = _clock.Normalize(endTime.Value),
            });
        }
        if (level.HasValue)
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, "level"), GetLogEventLevel(level.Value).ToString()));
        }
        if (!machineName.IsNullOrWhiteSpace())
        {
            // 模糊匹配
            queries.Add(new WildcardQuery(GetField(indexMappingInfo, "fields.MachineName"))
            {
                Value = $"*{machineName}*"
            });
        }
        if (!environment.IsNullOrWhiteSpace())
        {
            // 模糊匹配
            queries.Add(new WildcardQuery(GetField(indexMappingInfo, "fields.EnvironmentName"))
            {
                Value = $"*{environment}*"
            });
        }
        if (!application.IsNullOrWhiteSpace())
        {
            // 模糊匹配
            queries.Add(new WildcardQuery(GetField(indexMappingInfo, "fields.ApplicationName"))
            {
                Value = $"*{application}*"
            });
        }
        if (!context.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, "fields.SourceContext"), context));
        }
        if (!requestId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, "fields.RequestId"), requestId));
        }
        if (!requestPath.IsNullOrWhiteSpace())
        {
            // 前缀匹配
            queries.Add(new MatchPhrasePrefixQuery(GetField(indexMappingInfo, "fields.RequestPath"), requestPath));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            // 模糊匹配
            queries.Add(new WildcardQuery(GetField(indexMappingInfo, "fields.CorrelationId"))
            {
                Value = $"*{correlationId}*"
            });
        }
        if (processId.HasValue)
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, "fields.ProcessId"), FieldValue.FromValue(processId.Value)));
        }
        if (threadId.HasValue)
        {
            queries.Add(new TermQuery(GetField(indexMappingInfo, "fields.ThreadId"), FieldValue.FromValue(threadId.Value)));
        }

        if (hasException.HasValue)
        {
            if (hasException.Value)
            {
                /*  存在exceptions字段则就是有异常信息
                 * "exists": {
                        "field": "exceptions"
                    }
                 */
                queries.Add(new ExistsQuery(GetField(indexMappingInfo, "fields.Exceptions")));
            }
            else
            {
                // 不存在 exceptions字段就是没有异常信息的消息
                /*
                 * "bool": {
                        "must_not": [
                            {
                                "exists": {
                                    "field": "exceptions"
                                }
                            }
                        ]
                    }
                 */
                queries.Add(new BoolQuery
                {
                    MustNot = new List<Query>
                    {
                        new ExistsQuery(GetField(indexMappingInfo, "fields.Exceptions"))
                    }
                });
            }
        }

        return queries;
    }

    private async Task<List<SerilogInfo>> SearchFromSizeSerilogLogs(
        ElasticsearchClient client,
        Query query,
        SortOptions[]? sorts = null,
        int maxResultCount = 50,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var searchResponse = await client.SearchAsync<SerilogInfo>(dsl =>
        {
            dsl.Indices(CreateIndex())
                .Query(query)
               .From(skipCount)
               .Size(maxResultCount);
            if (sorts != null)
            {
                dsl.Sort(sorts);
            }
        }, cancellationToken);

        if (searchResponse.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query log failed: {errorMessage}", errorMessage);
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<SerilogInfo>> SearchAfterSerilogLogs(
        ElasticsearchClient client,
        Query query,
        SortOptions[] sorts,
        int maxResultCount = 50,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var searchAfter = await GetSearchAfterValue(
            client,
            query,
            sorts,
            skipCount,
            cancellationToken);

        if (searchAfter == null || !searchAfter.Any())
        {
            return [];
        }

        var searchResponse = await client.SearchAsync<SerilogInfo>(dsl =>
        {
            dsl.Indices(CreateIndex())
                .Query(query)
                .Sort(sorts)
                .Size(maxResultCount)
                .SearchAfter(searchAfter);
        }, cancellationToken);

        if (searchResponse.TryGetErrorMessage(out var errorMessage))
        {
            Logger.LogWarning("Query log failed: {errorMessage}", errorMessage);
            return [];
        }

        return searchResponse.Documents.ToList();
    }

    private async Task<List<FieldValue>?> GetSearchAfterValue(
        ElasticsearchClient client,
        Query query,
        SortOptions[] sorts,
        int skipCount,
        CancellationToken cancellationToken = default)
    {
        // 10000以内直接取最后一条数据
        if (skipCount < 10000)
        {
            var response = await client.SearchAsync<SerilogInfo>(
                dsl => dsl.Indices(CreateIndex())
                    .Query(query)
                    .Sort(sorts)
                    .SourceIncludes(x => x.Level)
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
        var firstResponse = await client.SearchAsync<SerilogInfo>(
            dsl => dsl.Indices(CreateIndex())
                    .Query(query)
                    .Sort(sorts)
                    .SourceIncludes(x => x.Level)
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
        var secondResponse = await client.SearchAsync<SerilogInfo>(
            dsl => dsl.Indices(CreateIndex())
                    .Query(query)
                    // 反转排序取第一个数据作为起始索引
                    .Sort(sorts.ReverseSort()!.ToArray())
                    .SourceIncludes(x => x.Level)
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

    protected virtual string CreateIndex(DateTimeOffset? offset = null)
    {
        if (!offset.HasValue)
        {
            return _indexFormatRegex.Replace(_options.IndexFormat, @"$1*$2");
        }
        return string.Format(_options.IndexFormat, offset.Value).ToLowerInvariant();
    }

    protected virtual LogEventLevel GetLogEventLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.None or LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Verbose,
        };
    }

    private static SortOptions[]? GetOrDefaultSort(IndexMappingInfo indexMappingInfo, string? sorting = null)
    {
        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
            ? SortOrder.Asc : SortOrder.Desc;
        sorting = !sorting.IsNullOrWhiteSpace()
            ? sorting.Split()[0]
            : ElasticsearchJsonFormatter.TimestampPropertyName;

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
