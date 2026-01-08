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
                c.Settings(_elasticsearchOptions.IndexSettings);
                c.Mappings(mp => mp
                    .Properties<AuditLog>(pd =>
                    {
                        pd.Keyword(k => k.Id);
                        pd.Keyword(k => k.ApplicationName);
                        pd.Keyword(k => k.UserId);
                        pd.Keyword(k => k.UserName);
                        pd.Keyword(k => k.TenantId);
                        pd.Keyword(k => k.TenantName);
                        pd.Keyword(k => k.ImpersonatorUserId);
                        pd.Keyword(k => k.ImpersonatorUserName);
                        pd.Keyword(k => k.ImpersonatorTenantId);
                        pd.Keyword(k => k.ImpersonatorTenantName);
                        pd.Date(d => d.ExecutionTime, d => d.Format(dateTimeFormat));
                        pd.IntegerNumber(n => n.ExecutionDuration);
                        pd.Keyword(k => k.ClientIpAddress);
                        pd.Keyword(k => k.ClientName);
                        pd.Keyword(k => k.ClientId);
                        pd.Keyword(k => k.CorrelationId);
                        pd.Keyword(k => k.BrowserInfo);
                        pd.Keyword(k => k.HttpMethod);
                        pd.Keyword(k => k.Url);
                        pd.Keyword(k => k.Exceptions);
                        pd.Keyword(k => k.Comments);
                        pd.IntegerNumber(n => n.HttpStatusCode);
                        pd.Nested(n => n.EntityChanges, np =>
                        {
                            np.Dynamic(DynamicMapping.False);
                            np.Properties(npd =>
                             {
                                 npd.Keyword(nameof(EntityChange.Id));
                                 npd.Keyword(nameof(EntityChange.AuditLogId));
                                 npd.Keyword(nameof(EntityChange.TenantId));
                                 npd.Date(nameof(EntityChange.ChangeTime), d => d.Format(dateTimeFormat));
                                 npd.IntegerNumber(nameof(EntityChange.ChangeType));
                                 npd.Keyword(nameof(EntityChange.EntityTenantId));
                                 npd.Keyword(nameof(EntityChange.EntityId));
                                 npd.Keyword(nameof(EntityChange.EntityTypeFullName));
                                 npd.Nested(nameof(EntityChange.PropertyChanges), pc =>
                                 {
                                     pc.Dynamic(DynamicMapping.False);
                                     pc.Properties(pcn =>
                                       {
                                           pcn.Keyword(nameof(EntityPropertyChange.Id));
                                           pcn.Keyword(nameof(EntityPropertyChange.TenantId));
                                           pcn.Keyword(nameof(EntityPropertyChange.EntityChangeId));
                                           pcn.Keyword(nameof(EntityPropertyChange.NewValue));
                                           pcn.Keyword(nameof(EntityPropertyChange.OriginalValue));
                                           pcn.Keyword(nameof(EntityPropertyChange.PropertyName));
                                           pcn.Keyword(nameof(EntityPropertyChange.PropertyTypeFullName));
                                       });
                                 });
                                 npd.Object(nameof(EntityChange.ExtraProperties));
                             });
                        });
                        pd.Nested(n => n.Actions, np =>
                        {
                            np.Dynamic(DynamicMapping.False);
                            np.Properties(npd =>
                            {
                                npd.Keyword(nameof(AuditLogAction.Id));
                                npd.Keyword(nameof(AuditLogAction.TenantId));
                                npd.Keyword(nameof(AuditLogAction.AuditLogId));
                                npd.Keyword(nameof(AuditLogAction.ServiceName));
                                npd.Keyword(nameof(AuditLogAction.MethodName));
                                npd.Keyword(nameof(AuditLogAction.Parameters));
                                npd.Date(nameof(AuditLogAction.ExecutionTime), d => d.Format(dateTimeFormat));
                                npd.IntegerNumber(nameof(AuditLogAction.ExecutionDuration));
                                npd.Object(nameof(AuditLogAction.ExtraProperties));
                            });
                        });
                        pd.Object(f => f.ExtraProperties);
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
                c.Settings(_elasticsearchOptions.IndexSettings);
                c.Mappings(mp =>
                {
                    mp.Properties<SecurityLog>(pd =>
                    {
                        pd.Keyword(k => k.Id);
                        pd.Keyword(k => k.TenantId);
                        pd.Keyword(k => k.ApplicationName);
                        pd.Keyword(k => k.Identity);
                        pd.Keyword(k => k.Action);
                        pd.Keyword(k => k.UserId);
                        pd.Keyword(k => k.UserName);
                        pd.Keyword(k => k.TenantName);
                        pd.Keyword(k => k.ClientId);
                        pd.Keyword(k => k.CorrelationId);
                        pd.Keyword(k => k.ClientIpAddress);
                        pd.Keyword(k => k.BrowserInfo);
                        pd.Date(k => k.CreationTime, d => d.Format(dateTimeFormat));
                        pd.Object(f => f.ExtraProperties);
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
