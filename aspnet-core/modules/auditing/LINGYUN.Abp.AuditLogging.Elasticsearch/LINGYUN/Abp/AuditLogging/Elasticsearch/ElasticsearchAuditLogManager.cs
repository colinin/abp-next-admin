using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    [Dependency(ReplaceServices = true)]
    public class ElasticsearchAuditLogManager : IAuditLogManager, ITransientDependency
    {
        private readonly AbpAuditingOptions _auditingOptions;
        private readonly AbpElasticsearchOptions _elasticsearchOptions;
        private readonly IIndexNameNormalizer _indexNameNormalizer;
        private readonly IElasticsearchClientFactory _clientFactory;
        private readonly IAuditLogInfoToAuditLogConverter _converter;

        public ILogger<ElasticsearchAuditLogManager> Logger { protected get; set; }

        public ElasticsearchAuditLogManager(
            IIndexNameNormalizer indexNameNormalizer,
            IOptions<AbpElasticsearchOptions> elasticsearchOptions,
            IElasticsearchClientFactory clientFactory,
            IOptions<AbpAuditingOptions> auditingOptions,
            IAuditLogInfoToAuditLogConverter converter)
        {
            _converter = converter;
            _clientFactory = clientFactory;
            _auditingOptions = auditingOptions.Value;
            _elasticsearchOptions = elasticsearchOptions.Value;
            _indexNameNormalizer = indexNameNormalizer;

            Logger = NullLogger<ElasticsearchAuditLogManager>.Instance;
        }


        public virtual async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            CancellationToken cancellationToken = default(CancellationToken))
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
                dsl.Index(CreateIndex())
                   .Query(log => log.Bool(b => b.Must(querys.ToArray()))),
                cancellationToken);

            return response.Count;
        }

        public virtual async Task<List<AuditLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.Create();

            var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
                ? SortOrder.Ascending : SortOrder.Descending;
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

            SourceFilterDescriptor<AuditLog> SourceFilter(SourceFilterDescriptor<AuditLog> selector)
            {
                selector.IncludeAll();
                if (!includeDetails)
                {
                    selector.Excludes(field =>
                        field.Field(f => f.Actions)
                             .Field(f => f.Comments)
                             .Field(f => f.Exceptions)
                             .Field(f => f.EntityChanges));
                }

                return selector;
            }

            var response = await client.SearchAsync<AuditLog>(dsl =>
                dsl.Index(CreateIndex())
                   .Query(log => log.Bool(b => b.Must(querys.ToArray())))
                   .Source(SourceFilter)
                   .Sort(log => log.Field(GetField(sorting), sortOrder))
                   .From(skipCount)
                   .Size(maxResultCount),
                cancellationToken);

            return response.Documents.ToList();
        }

        public virtual async Task<AuditLog> GetAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var client = _clientFactory.Create();

            var response = await client.GetAsync<AuditLog>(
                id,
                dsl =>
                    dsl.Index(CreateIndex()),
                cancellationToken);

            return response.Source;
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var client = _clientFactory.Create();

            await client.DeleteAsync<AuditLog>(
                id,
                dsl =>
                    dsl.Index(CreateIndex()),
                cancellationToken);
        }

        public virtual async Task<string> SaveAsync(
            AuditLogInfo auditInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!_auditingOptions.HideErrors)
            {
                return await SaveLogAsync(auditInfo, cancellationToken);
            }

            try
            {
                return await SaveLogAsync(auditInfo, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo.ToString());
                Logger.LogException(ex, Microsoft.Extensions.Logging.LogLevel.Error);
            }
            return "";
        }

        protected virtual async Task<string> SaveLogAsync(
            AuditLogInfo auditLogInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.Create();

            var auditLog = await _converter.ConvertAsync(auditLogInfo);

            var response = await client.IndexAsync(
                auditLog,
                (x) => x.Index(CreateIndex())
                        .Id(auditLog.Id),
                cancellationToken);

            return response.Id;
        }

        protected virtual List<Func<QueryContainerDescriptor<AuditLog>, QueryContainer>> BuildQueryDescriptor(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null)
        {
            var querys = new List<Func<QueryContainerDescriptor<AuditLog>, QueryContainer>>();

            if (startTime.HasValue)
            {
                querys.Add((log) => log.DateRange((q) => q.Field(GetField(nameof(AuditLog.ExecutionTime))).GreaterThanOrEquals(startTime)));
            }
            if (endTime.HasValue)
            {
                querys.Add((log) => log.DateRange((q) => q.Field(GetField(nameof(AuditLog.ExecutionTime))).LessThanOrEquals(endTime)));
            }
            if (!httpMethod.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.HttpMethod))).Value(httpMethod)));
            }
            if (!url.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Match((q) => q.Field(GetField(nameof(AuditLog.Url))).Query(url)));
            }
            if (userId.HasValue)
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.UserId))).Value(userId)));
            }
            if (!userName.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.UserName))).Value(userName)));
            }
            if (!applicationName.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.ApplicationName))).Value(applicationName)));
            }
            if (!correlationId.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.CorrelationId))).Value(correlationId)));
            }
            if (!clientId.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.ClientId))).Value(clientId)));
            }
            if (!clientIpAddress.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.ClientIpAddress))).Value(clientIpAddress)));
            }
            if (maxExecutionDuration.HasValue)
            {
                querys.Add((log) => log.Range((q) => q.Field(GetField(nameof(AuditLog.ExecutionDuration))).LessThanOrEquals(maxExecutionDuration)));
            }
            if (minExecutionDuration.HasValue)
            {
                querys.Add((log) => log.Range((q) => q.Field(GetField(nameof(AuditLog.ExecutionDuration))).GreaterThanOrEquals(minExecutionDuration)));
            }


            if (hasException.HasValue)
            {
                if (hasException.Value)
                {
                    querys.Add(
                        (q) => q.Bool(
                            (b) => b.Must(
                                (m) => m.Exists(
                                    (e) => e.Field((f) => f.Exceptions)))
                        )
                    );
                }
                else
                {
                    querys.Add(
                        (q) => q.Bool(
                            (b) => b.MustNot(
                                (mn) => mn.Exists(
                                    (e) => e.Field(
                                        (f) => f.Exceptions)))
                        )
                    );
                }
            }

            if (httpStatusCode.HasValue)
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(AuditLog.HttpStatusCode))).Value(httpStatusCode)));
            }

            return querys;
        }

        protected virtual string CreateIndex()
        {
            return _indexNameNormalizer.NormalizeIndex("audit-log");
        }

        private readonly static IDictionary<string, string> _fieldMaps = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
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
        protected virtual string GetField(string field)
        {
            if (_fieldMaps.TryGetValue(field, out string mapField))
            {
                return _elasticsearchOptions.FieldCamelCase ? mapField.ToCamelCase() : mapField.ToPascalCase();
            }

            return _elasticsearchOptions.FieldCamelCase ? field.ToCamelCase() : field.ToPascalCase();
        }
    }
}
