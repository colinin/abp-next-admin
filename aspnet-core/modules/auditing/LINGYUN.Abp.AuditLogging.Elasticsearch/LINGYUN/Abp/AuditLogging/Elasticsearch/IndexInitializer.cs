using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
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

        public virtual async Task InitializeAsync()
        {
            var client = _clientFactory.Create();
            var dateTimeFormat = !_jsonOptions.DefaultDateTimeFormat.IsNullOrWhiteSpace()
                ? $"{_jsonOptions.DefaultDateTimeFormat}||strict_date_optional_time||epoch_millis" 
                : "strict_date_optional_time||epoch_millis";
            var indexState = new IndexState
            {
                Settings = _elasticsearchOptions.IndexSettings,
            };
            await InitlizeAuditLogIndex(client, indexState, dateTimeFormat);
            await InitlizeSecurityLogIndex(client, indexState, dateTimeFormat);
        }

        protected virtual async Task InitlizeAuditLogIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("audit-log");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<AuditLog>(map =>
                                map.AutoMap()
                                   .Properties(mp =>
                                       mp.Date(p => p.Name(n => n.ExecutionTime).Format(dateTimeFormat))
                                         .Object<ExtraPropertyDictionary>(p => p.Name(n => n.ExtraProperties))
                                         .Nested<EntityChange>(n =>
                                            n.AutoMap()
                                             .Name(nameof(AuditLog.EntityChanges))
                                             .Properties(np =>
                                                np.Object<ExtraPropertyDictionary>(p => p.Name(n => n.ExtraProperties))
                                                  .Date(p => p.Name(n => n.ChangeTime).Format(dateTimeFormat))
                                                  .Nested<EntityPropertyChange>(npn => npn.Name(nameof(EntityChange.PropertyChanges)))))
                                         .Nested<AuditLogAction>(n => n.Name(nameof(AuditLog.Actions))
                                            .AutoMap()
                                            .Properties((np => 
                                                np.Object<ExtraPropertyDictionary>(p => p.Name(n => n.ExtraProperties))
                                                  .Date(p => p.Name(n => n.ExecutionTime).Format(dateTimeFormat))))))));
                if (!indexCreateResponse.IsValid)
                {
                    Logger.LogWarning("Failed to initialize index and audit log may not be retrieved.");
                    Logger.LogWarning(indexCreateResponse.OriginalException.ToString());
                }
            }
        }

        protected virtual async Task InitlizeSecurityLogIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("security-log");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<SecurityLog>(map => 
                                    map.AutoMap()
                                       .Properties(mp => 
                                            mp.Object<ExtraPropertyDictionary>(p => p.Name(n => n.ExtraProperties))
                                              .Date(p => p.Name(n => n.CreationTime).Format(dateTimeFormat)))));
                if (!indexCreateResponse.IsValid)
                {
                    Logger.LogWarning("Failed to initialize index and security log may not be retrieved.");
                    Logger.LogWarning(indexCreateResponse.OriginalException.ToString());
                }
            }
        }
    }
}
