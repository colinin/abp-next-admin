using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchSecurityLogManager : ISecurityLogManager, ITransientDependency
{
    private readonly AbpSecurityLogOptions _securityLogOptions;
    private readonly AbpElasticsearchOptions _elasticsearchOptions;
    private readonly AbpAuditLoggingElasticsearchOptions _loggingEsOptions;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IClock _clock;

    public ILogger<ElasticsearchSecurityLogManager> Logger { protected get; set; }

    public ElasticsearchSecurityLogManager(
        IClock clock,
        IGuidGenerator guidGenerator,
        IIndexNameNormalizer indexNameNormalizer,
        IOptions<AbpSecurityLogOptions> securityLogOptions,
        IOptions<AbpElasticsearchOptions> elasticsearchOptions,
        IElasticsearchClientFactory clientFactory,
        IOptionsMonitor<AbpAuditLoggingElasticsearchOptions> loggingEsOptions)
    {
        _clock = clock;
        _guidGenerator = guidGenerator;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _securityLogOptions = securityLogOptions.Value;
        _elasticsearchOptions = elasticsearchOptions.Value;
        _loggingEsOptions = loggingEsOptions.CurrentValue;

        Logger = NullLogger<ElasticsearchSecurityLogManager>.Instance;
    }

    public async virtual Task SaveAsync(
        SecurityLogInfo securityLogInfo,
        CancellationToken cancellationToken = default)
    {
        // TODO: 框架不把这玩意儿放在 ISecurityLogManager?
        if (!_securityLogOptions.IsEnabled)
        {
            return;
        }


        if (!_loggingEsOptions.IsSecurityLogEnabled)
        {
            Logger.LogInformation(securityLogInfo.ToString());
            return;
        }

        var client = _clientFactory.Create();

        var securityLog = new SecurityLog(
            _guidGenerator.Create(),
            securityLogInfo);

        var response = await client.IndexAsync(
            securityLog,
            (x) => x.Index(CreateIndex())
                    .Id(securityLog.Id),
            cancellationToken);

        if (!response.IsValidResponse)
        {
            Logger.LogWarning("Could not save the security log object: " + Environment.NewLine + securityLogInfo.ToString());
            if (response.TryGetOriginalException(out var ex))
            {
                Logger.LogWarning(ex, ex.Message);
            }
            else if (response.ElasticsearchServerError != null)
            {
                Logger.LogWarning(response.ElasticsearchServerError.ToString());
            }
        }
    }

    public async virtual Task<SecurityLog> GetAsync(
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

    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        await client.DeleteAsync<SecurityLog>(
            id,
            dsl =>
                dsl.Index(CreateIndex()),
            cancellationToken);
    }

    public async virtual Task DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var idValues = ids.Select(x => FieldValue.String(x.ToString())).ToList();
        await client.DeleteByQueryAsync<SecurityLog>(
            x => x.Indices(CreateIndex())
                  .Query(query =>
                    query.Terms(terms =>
                        terms.Field(field => field.Id)
                             .Terms(new TermsQueryField(idValues)))),
            cancellationToken);
    }

    public async virtual Task<List<SecurityLog>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
            ? SortOrder.Asc : SortOrder.Desc;
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
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               })
               .Sort(log => log.Field(GetField(sorting), sortOrder))
               .From(skipCount)
               .Size(maxResultCount),
            cancellationToken);

        return response.Documents.ToList();
    }


    public async virtual Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default)
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
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               }),
            cancellationToken);

        return response.Count;
    }

    protected virtual List<Query> BuildQueryDescriptor(
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
        var queries = new List<Query>();

        if (startTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(SecurityLog.CreationTime)))
            {
                Gte = _clock.Normalize(startTime.Value)
            });
        }
        if (endTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(SecurityLog.CreationTime)))
            {
                Lte = _clock.Normalize(endTime.Value)
            });
        }
        if (!applicationName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.ApplicationName)), applicationName));
        }
        if (!identity.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.Identity)), identity));
        }
        if (!action.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.Action)), action));
        }
        if (userId.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.UserId)), userId.Value.ToString()));
        }
        if (!userName.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.UserName)), userName));
        }
        if (!clientId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.ClientId)), clientId));
        }
        if (!clientIpAddress.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.ClientIpAddress)), clientIpAddress));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(SecurityLog.CorrelationId)), correlationId));
        }

        return queries;
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
