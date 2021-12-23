using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    [Dependency(ReplaceServices = true)]
    public class ElasticsearchSecurityLogManager : ISecurityLogManager, ITransientDependency
    {
        private readonly AbpSecurityLogOptions _securityLogOptions;
        private readonly AbpElasticsearchOptions _elasticsearchOptions;
        private readonly IIndexNameNormalizer _indexNameNormalizer;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IElasticsearchClientFactory _clientFactory;

        public ILogger<ElasticsearchSecurityLogManager> Logger { protected get; set; }

        public ElasticsearchSecurityLogManager(
            IGuidGenerator guidGenerator,
            IIndexNameNormalizer indexNameNormalizer,
            IOptions<AbpSecurityLogOptions> securityLogOptions,
            IOptions<AbpElasticsearchOptions> elasticsearchOptions,
            IElasticsearchClientFactory clientFactory)
        {
            _guidGenerator = guidGenerator;
            _clientFactory = clientFactory;
            _indexNameNormalizer = indexNameNormalizer;
            _securityLogOptions = securityLogOptions.Value;
            _elasticsearchOptions = elasticsearchOptions.Value;

            Logger = NullLogger<ElasticsearchSecurityLogManager>.Instance;
        }

        public virtual async Task SaveAsync(
            SecurityLogInfo securityLogInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: 框架不把这玩意儿放在 ISecurityLogManager?
            if (!_securityLogOptions.IsEnabled)
            {
                return;
            }

            var client = _clientFactory.Create();

            var securityLog = new SecurityLog(
                _guidGenerator.Create(),
                securityLogInfo);

            await client.IndexAsync(
                securityLog,
                (x) => x.Index(CreateIndex())
                        .Id(securityLog.Id),
                cancellationToken);
        }

        public virtual async Task<SecurityLog> GetAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var client = _clientFactory.Create();

            var response = await client.GetAsync<SecurityLog>(
                id,
                dsl =>
                    dsl.Index(CreateIndex()),
                cancellationToken);

            return response.Source;
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var client = _clientFactory.Create();

            await client.DeleteAsync<SecurityLog>(
                id,
                dsl =>
                    dsl.Index(CreateIndex()),
                cancellationToken);
        }

        public virtual async Task<List<SecurityLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.Create();

            var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
                ? SortOrder.Ascending : SortOrder.Descending;
            sorting = !sorting.IsNullOrWhiteSpace()
                ? sorting.Split()[0]
                : nameof(SecurityLog.CreationTime);

            var querys = BuildQueryDescriptor(
                startTime,
                endTime,
                applicationName,
                identity,
                action,
                userId,
                userName,
                clientId,
                clientIpAddress,
                correlationId);

            var response = await client.SearchAsync<SecurityLog>(dsl =>
                dsl.Index(CreateIndex())
                   .Query(log => log.Bool(b => b.Must(querys.ToArray())))
                   .Source(log => log.IncludeAll())
                   .Sort(log => log.Field(GetField(sorting), sortOrder))
                   .From(skipCount)
                   .Size(maxResultCount),
                cancellationToken);

            return response.Documents.ToList();
        }


        public virtual async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.Create();

            var querys = BuildQueryDescriptor(
                startTime,
                endTime,
                applicationName,
                identity,
                action,
                userId,
                userName,
                clientId,
                clientIpAddress,
                correlationId);

            var response = await client.CountAsync<SecurityLog>(dsl =>
                dsl.Index(CreateIndex())
                   .Query(log => log.Bool(b => b.Must(querys.ToArray()))),
                cancellationToken);

            return response.Count;
        }

        protected virtual List<Func<QueryContainerDescriptor<SecurityLog>, QueryContainer>> BuildQueryDescriptor(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null)
        {
            var querys = new List<Func<QueryContainerDescriptor<SecurityLog>, QueryContainer>>();

            if (startTime.HasValue)
            {
                querys.Add((log) => log.DateRange((q) => q.Field(GetField(nameof(SecurityLog.CreationTime))).GreaterThanOrEquals(startTime)));
            }
            if (endTime.HasValue)
            {
                querys.Add((log) => log.DateRange((q) => q.Field(GetField(nameof(SecurityLog.CreationTime))).LessThanOrEquals(endTime)));
            }
            if (!applicationName.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.ApplicationName))).Value(applicationName)));
            }
            if (!identity.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.Identity))).Value(identity)));
            }
            if (!action.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.Action))).Value(action)));
            }
            if (userId.HasValue)
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.UserId))).Value(userId)));
            }
            if (!userName.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.UserName))).Value(userName)));
            }
            if (!clientId.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.ClientId))).Value(clientId)));
            }
            if (!clientIpAddress.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.ClientIpAddress))).Value(clientIpAddress)));
            }
            if (!correlationId.IsNullOrWhiteSpace())
            {
                querys.Add((log) => log.Term((q) => q.Field(GetField(nameof(SecurityLog.CorrelationId))).Value(correlationId)));
            }

            return querys;
        }

        protected virtual string CreateIndex()
        {
            return _indexNameNormalizer.NormalizeIndex("security-log");
        }

        private readonly static IDictionary<string, string> _fieldMaps = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Id", "Id.keyword" },
            { "ApplicationName", "ApplicationName.keyword" },
            { "UserId", "UserId.keyword" },
            { "UserName", "UserName.keyword" },
            { "TenantId", "TenantId.keyword" },
            { "TenantName", "TenantName.keyword" },
            { "Identity", "Identity.keyword" },
            { "Action", "Action.keyword" },
            { "BrowserInfo", "BrowserInfo.keyword" },
            { "ClientIpAddress", "ClientIpAddress.keyword" },
            { "ClientId", "ClientId.keyword" },
            { "CorrelationId", "CorrelationId.keyword" },
            { "CreationTime", "CreationTime" },
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
