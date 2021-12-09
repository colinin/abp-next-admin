using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public class PersistenceIndexInitializer : IPersistenceIndexInitializer, ISingletonDependency
    {
        private readonly ILogger<PersistenceIndexInitializer> _logger;
        private readonly AbpJsonOptions _jsonOptions;
        private readonly AbpWorkflowCorePersistenceElasticsearchOptions _elasticsearchOptions;
        private readonly IPersistenceIndexNameNormalizer _nameNormalizer;
        private readonly IElasticsearchClientFactory _clientFactory;

        public PersistenceIndexInitializer(
           IOptions<AbpJsonOptions> jsonOptions,
           IOptions<AbpWorkflowCorePersistenceElasticsearchOptions> elasticsearchOptions,
           IPersistenceIndexNameNormalizer nameNormalizer,
           IElasticsearchClientFactory clientFactory,
           ILogger<PersistenceIndexInitializer> logger)
        {
            _jsonOptions = jsonOptions.Value;
            _elasticsearchOptions = elasticsearchOptions.Value;
            _nameNormalizer = nameNormalizer;
            _clientFactory = clientFactory;
            _logger = logger;
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

            await InitlizeWorkflowInstanceIndex(client, indexState, dateTimeFormat);
            await InitlizeEventIndex(client, indexState, dateTimeFormat);
            await InitlizeEventSubscriptionIndex(client, indexState, dateTimeFormat);
            await InitlizeExecutionErrorIndex(client, indexState, dateTimeFormat);
            await InitlizeScheduledCommandIndex(client, indexState, dateTimeFormat);
        }

        protected virtual async Task InitlizeWorkflowInstanceIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("instances");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<WorkflowInstance>(map => map.AutoMap()
                                .Properties(mp =>
                                    mp.Date(p => p.Name(n => n.CreateTime).Format(dateTimeFormat))
                                      .Date(p => p.Name(n => n.CompleteTime).Format(dateTimeFormat))
                                      .Nested<ExecutionPointer>(p => p.Name(n => n.ExecutionPointers)
                                         .AutoMap()
                                         .Properties(np =>
                                            np.Date(p => p.Name(n => n.EndTime).Format(dateTimeFormat))
                                              .Date(p => p.Name(n => n.StartTime).Format(dateTimeFormat))
                                              .Date(p => p.Name(n => n.SleepUntil).Format(dateTimeFormat))
                                              .Object<Dictionary<string, object>>(p => p.Name(n => n.ExtensionAttributes)))))));

                CheckResponse(indexCreateResponse);
            }
        }

        protected virtual async Task InitlizeEventIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("events");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<Event>(map => map.AutoMap()
                                .Properties(mp =>
                                    mp.Date(p => p.Name(n => n.EventTime).Format(dateTimeFormat)))));

                CheckResponse(indexCreateResponse);
            }
        }

        protected virtual async Task InitlizeEventSubscriptionIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("subscriptions");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<EventSubscription>(map => map.AutoMap()
                                .Properties(mp =>
                                    mp.Date(p => p.Name(n => n.SubscribeAsOf).Format(dateTimeFormat))
                                      .Date(p => p.Name(n => n.ExternalTokenExpiry).Format(dateTimeFormat)))));

                CheckResponse(indexCreateResponse);
            }
        }

        protected virtual async Task InitlizeExecutionErrorIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("executionerrors");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<ExecutionError>(map => map.AutoMap()
                                .Properties(mp =>
                                    mp.Date(p => p.Name(n => n.ErrorTime).Format(dateTimeFormat)))));

                CheckResponse(indexCreateResponse);
            }
        }

        protected virtual async Task InitlizeScheduledCommandIndex(IElasticClient client, IIndexState indexState, string dateTimeFormat)
        {
            var indexName = _nameNormalizer.NormalizeIndex("scheduledcommands");
            var indexExists = await client.Indices.ExistsAsync(indexName);
            if (!indexExists.Exists)
            {
                var indexCreateResponse = await client.Indices.CreateAsync(
                    indexName,
                    dsl => dsl.InitializeUsing(indexState)
                              .Map<PersistedScheduledCommand>(map => map.AutoMap()));

                CheckResponse(indexCreateResponse);
            }
        }

        private void CheckResponse(IResponse response)
        {
            if (!response.ApiCall.Success)
            {
                _logger.LogError(default(EventId), response.ApiCall.OriginalException, $"ES Persistence index initlize failed");
                throw new AbpException($"ES Operation Failed", response.ApiCall.OriginalException);
            }

            if (!response.IsValid)
            {
                _logger.LogWarning("ES Persistence index initlize valid error: {0}", response.DebugInformation);
                throw new InvalidOperationException(response.DebugInformation, response.OriginalException);
            }
        }
    }
}
