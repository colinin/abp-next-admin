using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    [Dependency(ReplaceServices = true)]
    public class SerilogElasticsearchLoggingManager : ILoggingManager, ISingletonDependency
    {
        private static readonly Regex IndexFormatRegex = new Regex(@"^(.*)(?:\{0\:.+\})(.*)$");

        private readonly IObjectMapper _objectMapper;
        private readonly ICurrentTenant _currentTenant;
        private readonly AbpLoggingSerilogElasticsearchOptions _options;
        private readonly IElasticsearchClientFactory _clientFactory;

        public ILogger<SerilogElasticsearchLoggingManager> Logger { protected get; set; }

        public SerilogElasticsearchLoggingManager(
            IObjectMapper objectMapper,
            ICurrentTenant currentTenant,
            IOptions<AbpLoggingSerilogElasticsearchOptions> options,
            IElasticsearchClientFactory clientFactory)
        {
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
        public virtual async Task<LogInfo> GetAsync(
            string id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.Create();

            ISearchResponse<SerilogInfo> response;

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
                                    "@timestamp": {
                                        "value": "2021-10-31T09:53:12.3406273+08:00"
                                    }
                                }
                            }
                        ]
                    }
                }
                */
                response = await client.SearchAsync<SerilogInfo>(
                    dsl =>
                        dsl.Index(CreateIndex())
                           .Query(
                                (q) => q.Bool(
                                    (b) => b.Must(
                                        (s) => s.Term(
                                            (t) => t.Field("@timestamp").Value(id)),
                                        (s) => s.Term(
                                            (t) => t.Field(f => f.Fields.TenantId.Suffix("keyword")).Value(_currentTenant.GetId()))))),
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
                                    "@timestamp": {
                                        "value": "2021-10-31T09:53:12.3406273+08:00"
                                    }
                                }
                            }
                        ]
                    }
                }
                */
                response = await client.SearchAsync<SerilogInfo>(
                    dsl =>
                        dsl.Index(CreateIndex())
                           .Query(
                                (q) => q.Bool(
                                    (b) => b.Must(
                                        (s) => s.Term(
                                            (t) => t.Field("@timestamp").Value(id))))),
                    cancellationToken);
            }

            return _objectMapper.Map<SerilogInfo, LogInfo>(response.Documents.FirstOrDefault());
        }

        public virtual async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            Microsoft.Extensions.Logging.LogLevel? level = null,
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
            CancellationToken cancellationToken = default(CancellationToken))
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
                dsl.Index(CreateIndex())
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
        public virtual async Task<List<LogInfo>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            Microsoft.Extensions.Logging.LogLevel? level = null,
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
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.Create();

            var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
                ? SortOrder.Ascending : SortOrder.Descending;
            sorting = sorting ?? "timestamp";

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

            SourceFilterDescriptor<SerilogInfo> SourceFilter(SourceFilterDescriptor<SerilogInfo> selector)
            {
                selector.IncludeAll();
                if (!includeDetails)
                {
                    selector.Excludes(field =>
                        field.Field("exceptions"));
                }

                return selector;
            }

            var response = await client.SearchAsync<SerilogInfo>((dsl) =>
                dsl.Index(CreateIndex())
                   .Query(log => log.Bool(b => b.Must(querys.ToArray())))
                   .Source(SourceFilter)
                   .Sort(log => log.Field(GetField(sorting), sortOrder))
                   .From(skipCount)
                   .Size(maxResultCount),
                cancellationToken);

            return _objectMapper.Map<List<SerilogInfo>, List<LogInfo>>(response.Documents.ToList());
        }

        protected virtual List<Func<QueryContainerDescriptor<SerilogInfo>, QueryContainer>> BuildQueryDescriptor(
            DateTime? startTime = null,
            DateTime? endTime = null,
            Microsoft.Extensions.Logging.LogLevel? level = null,
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
            var querys = new List<Func<QueryContainerDescriptor<SerilogInfo>, QueryContainer>>();

            if (_currentTenant.IsAvailable)
            {
                querys.Add((log) => log.Term((q) => q.Field((f) => f.Fields.TenantId.Suffix("keyword")).Value(_currentTenant.GetId())));
            }
            if (startTime.HasValue)
            {
                querys.Add((log) => log.DateRange((q) => q.Field(f => f.TimeStamp).GreaterThanOrEquals(startTime)));
            }
            if (endTime.HasValue)
            {
                querys.Add((log) => log.DateRange((q) => q.Field(f => f.TimeStamp).LessThanOrEquals(endTime)));
            }
            if (level.HasValue)
            {
                querys.Add((log) => log.Term((q) => q.Field(f => f.Level.Suffix("keyword")).Value(level.ToString())));
            }
            if (!machineName.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field((f) => f.Fields.MachineName.Suffix("keyword")).Value(machineName)));
            }
            if (!environment.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field((f) => f.Fields.Environment.Suffix("keyword")).Value(environment)));
            }
            if (!application.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field((f) => f.Fields.Application.Suffix("keyword")).Value(application)));
            }
            if (!context.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field((f) => f.Fields.Context.Suffix("keyword")).Value(context)));
            }
            if (!requestId.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Match((q) => q.Field(f => f.Fields.RequestId.Suffix("keyword")).Query(requestId)));
            }
            if (!requestPath.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(f => f.Fields.RequestPath.Suffix("keyword")).Value(requestPath)));
            }
            if (!correlationId.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(f => f.Fields.CorrelationId.Suffix("keyword")).Value(correlationId)));
            }
            if (processId.HasValue)
            {
                querys.Add((log) => log.Term((q) => q.Field(f => f.Fields.ProcessId).Value(processId)));
            }
            if (threadId.HasValue)
            {
                querys.Add((log) => log.Term((q) => q.Field(f => f.Fields.ThreadId).Value(threadId)));
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
                    querys.Add(
                        (q) => q.Exists(
                            (e) => e.Field("exceptions")));
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
                    querys.Add(
                        (q) => q.Bool(
                            (b) => b.MustNot(
                                (m) => m.Exists(
                                    (e) => e.Field("exceptions")))));
                }
            }

            return querys;
        }

        protected virtual string CreateIndex(DateTimeOffset? offset = null)
        {
            if (!offset.HasValue)
            {
                return IndexFormatRegex.Replace(_options.IndexFormat, @"$1*$2");
            }
            return string.Format(_options.IndexFormat, offset.Value).ToLowerInvariant();
        }

        private readonly static IDictionary<string, string> _fieldMaps = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "timestamp", "@timestamp" },
            { "level", "level.keyword" },
            { "machinename", $"fields.{AbpLoggingEnricherPropertyNames.MachineName}.keyword" },
            { "environment", $"fields.{AbpLoggingEnricherPropertyNames.EnvironmentName}.keyword" },
            { "application", $"fields.{AbpSerilogEnrichersConsts.ApplicationNamePropertyName}.keyword" },
            { "context", "fields.Context.keyword" },
            { "actionid", "fields.ActionId.keyword" },
            { "actionname", "fields.ActionName.keyword" },
            { "requestid", "fields.RequestId.keyword" },
            { "requestpath", "fields.RequestPath.keyword" },
            { "connectionid", "fields.ConnectionId.keyword" },
            { "correlationid", "fields.CorrelationId.keyword" },
            { "clientid", "fields.ClientId.keyword" },
            { "userid", "fields.UserId.keyword" },
            { "processid", "fields.ProcessId" },
            { "threadid", "fields.ThreadId" },
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
}
