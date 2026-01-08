using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class SerilogElasticsearchLoggingManager : ILoggingManager, ISingletonDependency
{
    private readonly static Regex IndexFormatRegex = new Regex(@"^(.*)(?:\{0\:.+\})(.*)$");

    private readonly IClock _clock;
    private readonly IObjectMapper _objectMapper;
    private readonly ICurrentTenant _currentTenant;
    private readonly AbpLoggingSerilogElasticsearchOptions _options;
    private readonly IElasticsearchClientFactory _clientFactory;

    public ILogger<SerilogElasticsearchLoggingManager> Logger { protected get; set; }

    public SerilogElasticsearchLoggingManager(
        IClock clock,
        IObjectMapper objectMapper,
        ICurrentTenant currentTenant,
        IOptions<AbpLoggingSerilogElasticsearchOptions> options,
        IElasticsearchClientFactory clientFactory)
    {
        _clock = clock;
        _objectMapper = objectMapper;
        _currentTenant = currentTenant;
        _clientFactory = clientFactory;
        _options = options.Value;

        Logger = NullLogger<SerilogElasticsearchLoggingManager>.Instance;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">时间类型或者转换为timestamp都可以查询</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async virtual Task<LogInfo> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

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
                                        (t) => t.Field(GetField(nameof(SerilogInfo.Fields.UniqueId))).Value(id)),
                                    (s) => s.Term(
                                        (t) => t.Field(GetField(nameof(SerilogInfo.Fields.TenantId))).Value(_currentTenant.GetId().ToString())))))
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
                                        (t) => t.Field(GetField(nameof(SerilogInfo.Fields.UniqueId))).Value(id)))))
                       .Size(1),
                cancellationToken);
        }

        return _objectMapper.Map<SerilogInfo, LogInfo>(response.Documents.FirstOrDefault());
    }

    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string machineName = null,
        string environment = null,
        string application = null,
        string context = null,
        string requestId = null,
        string requestPath = null,
        string correlationId = null,
        int? processId = null,
        int? threadId = null,
        bool? hasException = null,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var querys = BuildQueryDescriptor(
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
            dsl.Indices(CreateIndex())
               .Query(log => log.Bool(b => b.Must(querys.ToArray()))),
            cancellationToken);

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
        string sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string machineName = null,
        string environment = null,
        string application = null,
        string context = null,
        string requestId = null,
        string requestPath = null,
        string correlationId = null,
        int? processId = null,
        int? threadId = null,
        bool? hasException = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
            ? SortOrder.Asc : SortOrder.Desc;
        sorting = !sorting.IsNullOrWhiteSpace()
            ? sorting.Split()[0]
            : nameof(SerilogInfo.TimeStamp);

        var querys = BuildQueryDescriptor(
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

        var response = await client.SearchAsync<SerilogInfo>((dsl) =>
            dsl.Indices(CreateIndex())
               .Query(log =>
                    log.Bool(b =>
                        b.Must(querys.ToArray())))
               .SourceExcludes(se => se.Exceptions)
               .Sort(log => log.Field(GetField(sorting), sortOrder))
               .From(skipCount)
               .Size(maxResultCount),
            cancellationToken);

        return _objectMapper.Map<List<SerilogInfo>, List<LogInfo>>(response.Documents.ToList());
    }

    protected virtual List<Query> BuildQueryDescriptor(
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string machineName = null,
        string environment = null,
        string application = null,
        string context = null,
        string requestId = null,
        string requestPath = null,
        string correlationId = null,
        int? processId = null,
        int? threadId = null,
        bool? hasException = null)
    {
        var queries = new List<Query>();

        if (_currentTenant.IsAvailable)
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.TenantId)), _currentTenant.GetId().ToString()));
        }
        if (startTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(SerilogInfo.TimeStamp)))
            {
                Gte = _clock.Normalize(startTime.Value),
            });
        }
        if (endTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(SerilogInfo.TimeStamp)))
            {
                Lte = _clock.Normalize(endTime.Value),
            });
        }
        if (level.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Level)), GetLogEventLevel(level.Value).ToString()));
        }
        if (!machineName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.MachineName)), machineName));
        }
        if (!environment.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.Environment)), environment));
        }
        if (!application.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.Application)), application));
        }
        if (!context.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.Context)), context));
        }
        if (!requestId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.RequestId)), requestId));
        }
        if (!requestPath.IsNullOrWhiteSpace())
        {
            // 前缀匹配
            queries.Add(new MatchPhrasePrefixQuery(GetField(nameof(SerilogInfo.Fields.RequestPath)), requestPath));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            // 模糊匹配
            queries.Add(new WildcardQuery(GetField(nameof(SerilogInfo.Fields.CorrelationId)))
            {
                Value = $"*{correlationId}*"
            });
        }
        if (processId.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.ProcessId)), FieldValue.FromValue(processId.Value)));
        }
        if (threadId.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(SerilogInfo.Fields.ThreadId)), FieldValue.FromValue(threadId.Value)));
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
                queries.Add(new ExistsQuery(GetField("Exceptions")));
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
                        new ExistsQuery(GetField("Exceptions"))
                    }
                });
            }
        }

        return queries;
    }

    protected virtual string CreateIndex(DateTimeOffset? offset = null)
    {
        if (!offset.HasValue)
        {
            return IndexFormatRegex.Replace(_options.IndexFormat, @"$1*$2");
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

    private readonly static IDictionary<string, string> _fieldMaps = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
    {
        { "timestamp", "@timestamp" },
        { "level", "level.raw" },
        { "machinename", $"fields.{AbpLoggingEnricherPropertyNames.MachineName}.raw" },
        { "environment", $"fields.{AbpLoggingEnricherPropertyNames.EnvironmentName}.raw" },
        { "application", $"fields.{AbpSerilogEnrichersConsts.ApplicationNamePropertyName}.raw" },
        { "context", "fields.SourceContext.raw" },
        { "actionid", "fields.ActionId.raw" },
        { "actionname", "fields.ActionName.raw" },
        { "requestid", "fields.RequestId.raw" },
        { "requestpath", "fields.RequestPath" },
        { "connectionid", "fields.ConnectionId" },
        { "correlationid", "fields.CorrelationId.raw" },
        { "clientid", "fields.ClientId.raw" },
        { "userid", "fields.UserId.raw" },
        { "processid", "fields.ProcessId" },
        { "threadid", "fields.ThreadId" },
        { "id", $"fields.{AbpSerilogUniqueIdConsts.UniqueIdPropertyName}" },
        { "uniqueid", $"fields.{AbpSerilogUniqueIdConsts.UniqueIdPropertyName}" },
    };
    protected virtual string GetField(string field)
    {
        foreach (var fieldMap in _fieldMaps)
        {
            if (field.ToLowerInvariant().Contains(fieldMap.Key))
            {
                return fieldMap.Value;
            }
        }

        return field;
    }
}
