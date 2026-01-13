using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public class IndexInitializer : IIndexInitializer, ISingletonDependency
{
    private readonly AbpJsonOptions _jsonOptions;
    private readonly AbpAuditLoggingElasticsearchOptions _elasticsearchOptions;
    private readonly IIndexNameNormalizer _nameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;

    public ILogger<IndexInitializer> Logger { protected get; set; }

    public IndexInitializer(
        IOptions<AbpJsonOptions> jsonOptions,
        IOptions<AbpAuditLoggingElasticsearchOptions> elasticsearchOptions,
        IIndexNameNormalizer nameNormalizer,
        IElasticsearchClientFactory clientFactory)
    {
        _jsonOptions = jsonOptions.Value;
        _elasticsearchOptions = elasticsearchOptions.Value;
        _nameNormalizer = nameNormalizer;
        _clientFactory = clientFactory;

        Logger = NullLogger<IndexInitializer>.Instance;
    }

    public async virtual Task InitializeAsync()
    {
        var client = _clientFactory.Create();
        var dateTimeFormat = !_jsonOptions.OutputDateTimeFormat.IsNullOrWhiteSpace()
            ? $"{_jsonOptions.OutputDateTimeFormat}||strict_date_optional_time||epoch_millis" 
            : "strict_date_optional_time||epoch_millis";
        await InitlizeAuditLogIndex(client, dateTimeFormat);
        await InitlizeSecurityLogIndex(client, dateTimeFormat);
    }

    protected async virtual Task InitlizeAuditLogIndex(ElasticsearchClient client, string dateTimeFormat)
    {
        var indexName = _nameNormalizer.NormalizeIndex("audit-log");
        var indexExists = await client.Indices.ExistsAsync(indexName);
        if (!indexExists.Exists)
        {
            var indexCreateResponse = await client.Indices.CreateAsync(indexName, c =>
            {
                c.Settings(_elasticsearchOptions.AuditLogSettings);
                c.Mappings(mp => mp
                    .Dynamic(DynamicMapping.False)
                    .Properties<AuditLog>(pd =>
                    {
                        pd.Text(k => k.Id, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(t => t.ApplicationName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(64))));
                        pd.Text(k => k.UserId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(t => t.UserName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.TenantId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(t => t.TenantName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(64))));
                        pd.Text(k => k.ImpersonatorUserId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(t => t.ImpersonatorUserName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.ImpersonatorTenantId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(t => t.ImpersonatorTenantName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(64))));
                        pd.Date(d => d.ExecutionTime, d => d.Format(dateTimeFormat));
                        pd.IntegerNumber(n => n.ExecutionDuration);
                        pd.Text(k => k.ClientIpAddress, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.ClientName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.ClientId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.CorrelationId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(64))));
                        pd.Text(k => k.BrowserInfo, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(512))));
                        pd.Text(k => k.HttpMethod, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(k => k.Url, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(1024))));
                        pd.Text(k => k.Exceptions, p => p.Store(true).Index(false));
                        pd.Text(k => k.Comments, p => p.Store(true).Index(false));
                        pd.IntegerNumber(n => n.HttpStatusCode);
                        pd.Nested(n => n.EntityChanges, np =>
                        {
                            np.Dynamic(DynamicMapping.False);
                            np.Properties(npd =>
                             {
                                 npd.Text(nameof(EntityChange.Id), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                 npd.Text(nameof(EntityChange.AuditLogId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                 npd.Text(nameof(EntityChange.TenantId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                 npd.Date(nameof(EntityChange.ChangeTime), d => d.Format(dateTimeFormat));
                                 npd.ByteNumber(nameof(EntityChange.ChangeType));
                                 npd.Text(nameof(EntityChange.EntityTenantId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                 npd.Text(nameof(EntityChange.EntityId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(128))));
                                 npd.Text(nameof(EntityChange.EntityTypeFullName), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                 npd.Nested(nameof(EntityChange.PropertyChanges), pc =>
                                 {
                                     pc.Dynamic(DynamicMapping.False);
                                     pc.Properties(pcn =>
                                       {
                                           pcn.Text(nameof(EntityPropertyChange.Id), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                           pcn.Text(nameof(EntityPropertyChange.TenantId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                           pcn.Text(nameof(EntityPropertyChange.EntityChangeId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                           pcn.Text(nameof(EntityPropertyChange.NewValue), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                           pcn.Text(nameof(EntityPropertyChange.OriginalValue), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                           pcn.Text(nameof(EntityPropertyChange.PropertyName), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                           pcn.Text(nameof(EntityPropertyChange.PropertyTypeFullName), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                       });
                                 });
                                 npd.Flattened(nameof(EntityChange.ExtraProperties), f => f.DepthLimit(5).EagerGlobalOrdinals(false));
                             });
                        });
                        pd.Nested(n => n.Actions, np =>
                        {
                            np.Dynamic(DynamicMapping.False);
                            np.Properties(npd =>
                            {
                                npd.Text(nameof(AuditLogAction.Id), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                npd.Text(nameof(AuditLogAction.TenantId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                npd.Text(nameof(AuditLogAction.AuditLogId), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                                npd.Text(nameof(AuditLogAction.ServiceName), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                npd.Text(nameof(AuditLogAction.MethodName), p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                                npd.Text(nameof(AuditLogAction.Parameters), p => p.Store(true).Index(false));
                                npd.Date(nameof(AuditLogAction.ExecutionTime), d => d.Format(dateTimeFormat));
                                npd.IntegerNumber(nameof(AuditLogAction.ExecutionDuration));
                                npd.Flattened(nameof(AuditLogAction.ExtraProperties), f => f.DepthLimit(5).EagerGlobalOrdinals(false));
                            });
                        });
                        pd.Flattened(f => f.ExtraProperties, f => f.DepthLimit(5).EagerGlobalOrdinals(false));
                    }));
            });

            if (!indexCreateResponse.IsValidResponse)
            {
                if (indexCreateResponse.TryGetOriginalException(out var ex))
                {
                    Logger.LogWarning(ex, "Failed to initialize index and audit log may not be retrieved.");
                    return;
                }
                Logger.LogWarning("Failed to initialize index and audit log may not be retrieved.");
                Logger.LogWarning(indexCreateResponse.DebugInformation);
            }
        }
    }

    protected async virtual Task InitlizeSecurityLogIndex(ElasticsearchClient client, string dateTimeFormat)
    {
        var indexName = _nameNormalizer.NormalizeIndex("security-log");
        var indexExists = await client.Indices.ExistsAsync(indexName);
        if (!indexExists.Exists)
        {
            var indexCreateResponse = await client.Indices.CreateAsync(indexName, c =>
            {
                c.Settings(_elasticsearchOptions.SecurityLogSettings);
                c.Mappings(mp =>
                {
                    mp.Dynamic(DynamicMapping.False);
                    mp.Properties<SecurityLog>(pd =>
                    {
                        pd.Text(k => k.Id, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(k => k.TenantId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(k => k.ApplicationName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.Identity, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.Action, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.UserId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(36))));
                        pd.Text(k => k.UserName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(64))));
                        pd.Text(k => k.TenantName, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(64))));
                        pd.Text(k => k.ClientId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.CorrelationId, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.ClientIpAddress, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(256))));
                        pd.Text(k => k.BrowserInfo, p => p.Fields(f => f.Keyword("keyword", k => k.IgnoreAbove(512))));
                        pd.Date(k => k.CreationTime, d => d.Format(dateTimeFormat));
                        pd.Flattened(f => f.ExtraProperties, f => f.DepthLimit(5).EagerGlobalOrdinals(false));
                    });
                });
            });
            if (!indexCreateResponse.IsValidResponse)
            {
                if (indexCreateResponse.TryGetOriginalException(out var ex))
                {
                    Logger.LogWarning(ex, "Failed to initialize index and audit log may not be retrieved.");
                    return;
                }
                Logger.LogWarning("Failed to initialize index and audit log may not be retrieved.");
                Logger.LogWarning(indexCreateResponse.DebugInformation);
            }
        }
    }
}
