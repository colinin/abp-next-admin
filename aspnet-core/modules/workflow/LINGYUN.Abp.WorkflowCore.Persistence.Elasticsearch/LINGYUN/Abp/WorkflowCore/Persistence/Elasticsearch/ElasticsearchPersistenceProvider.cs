using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch.Models;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public class ElasticsearchPersistenceProvider : IPersistenceProvider, ITransientDependency
    {
        private readonly ILogger<ElasticsearchPersistenceProvider> _logger;

        private readonly IGuidGenerator _guidGenerator;
        private readonly IPersistenceIndexNameNormalizer _indexNameNormalizer;
        private readonly IPersistenceIndexInitializer _indexInitializer;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public ElasticsearchPersistenceProvider(
            IGuidGenerator guidGenerator,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IPersistenceIndexInitializer indexInitializer,
            IPersistenceIndexNameNormalizer indexNameNormalizer,
            ILogger<ElasticsearchPersistenceProvider> logger)
        {
            _guidGenerator = guidGenerator;
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _indexInitializer = indexInitializer;
            _indexNameNormalizer = indexNameNormalizer;
            _logger = logger;
        }

        public bool SupportsScheduledCommands => true;

        public virtual async Task ClearSubscriptionToken(string eventSubscriptionId, string token, CancellationToken cancellationToken = default)
        {
            var id = Guid.Parse(eventSubscriptionId);

            var client = CreateClient();

            var response = await client.GetAsync<EventSubscription>(
                id,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex)),
                ct: cancellationToken);

            CheckResponse(response);

            if (response.Found)
            {
                if (response.Source.ExternalToken != token)
                {
                    throw new InvalidOperationException();
                }
                response.Source.ExternalToken = null;
                response.Source.ExternalWorkerId = null;
                response.Source.ExternalTokenExpiry = null;

                await client.UpdateAsync<EventSubscription>(
                    id,
                    dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex))
                              .Doc(response.Source),
                    ct: cancellationToken);
            }
        }

        public virtual async Task<string> CreateEvent(Event newEvent, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var newEventId = _guidGenerator.Create();

            newEvent.Id = newEventId.ToString();

            var response = await client.IndexAsync(
                newEvent,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventIndex))
                          .Id(newEventId),
                ct: cancellationToken);

            CheckResponse(response);

            return newEvent.Id;
        }

        public virtual async Task<string> CreateEventSubscription(EventSubscription subscription, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var newSubscriptionId = _guidGenerator.Create();

            subscription.Id = newSubscriptionId.ToString();

            var response = await client.IndexAsync(
                subscription,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex))
                          .Id(newSubscriptionId),
                ct: cancellationToken);

            CheckResponse(response);

            return subscription.Id;
        }

        public virtual async Task<string> CreateNewWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var newWorkflowId = _guidGenerator.Create();

            workflow.Id = newWorkflowId.ToString();

            var response = await client.IndexAsync(
                workflow,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.WorkflowInstanceIndex))
                          .Id(newWorkflowId),
                ct: cancellationToken);

            CheckResponse(response);

            return workflow.Id;
        }

        public void EnsureStoreExists()
        {
            AsyncHelper.RunSync(async () => await _indexInitializer.InitializeAsync());
        }

        public virtual async Task<Event> GetEvent(string id, CancellationToken cancellationToken = default)
        {
            var eventId = Guid.Parse(id);

            var client = CreateClient();

            var response = await client.GetAsync<Event>(
                eventId,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventIndex)),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Source;
        }

        public virtual async Task<IEnumerable<string>> GetEvents(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var terms = new List<Func<QueryContainerDescriptor<Event>, QueryContainer>>();

            terms.Add(x => x.Term(t => t.Field(f => f.EventName.Suffix("keyword")).Value(eventName)));
            terms.Add(x => x.Term(t => t.Field(f => f.EventKey.Suffix("keyword")).Value(eventKey)));
            terms.Add(x => x.DateRange(t => t.Field(f => f.EventTime).GreaterThanOrEquals(asOf)));

            var response = await client.SearchAsync<Event>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventIndex))
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.Includes(e => e.Field(f => f.Id))),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Documents.Select(x => x.Id);
        }

        public virtual async Task<EventSubscription> GetFirstOpenSubscription(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var terms = new List<Func<QueryContainerDescriptor<EventSubscription>, QueryContainer>>();

            terms.Add(x => x.Term(t => t.Field(f => f.EventName.Suffix("keyword")).Value(eventName)));
            terms.Add(x => x.Term(t => t.Field(f => f.EventKey.Suffix("keyword")).Value(eventKey)));
            terms.Add(x => x.Term(t => t.Field(f => f.ExternalToken.Suffix("keyword")).Value(null)));
            terms.Add(x => x.DateRange(t => t.Field(f => f.SubscribeAsOf).LessThanOrEquals(asOf)));

            var response = await client.SearchAsync<EventSubscription>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex))
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.Includes(e => e.Field(f => f.Id)))
                    .Sort(s => s.Field(f => f.SubscribeAsOf, SortOrder.Ascending))
                    .Take(1),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Documents.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<string>> GetRunnableEvents(DateTime asAt, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();
            var now = asAt.ToUniversalTime();

            var terms = new List<Func<QueryContainerDescriptor<Event>, QueryContainer>>();

            terms.Add(x => x.Term(t => t.Field(f => f.IsProcessed).Value(false)));
            terms.Add(x => x.DateRange(t => t.Field(f => f.EventTime).LessThanOrEquals(now)));

            var response = await client.SearchAsync<Event>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventIndex))
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.Includes(e => e.Field(f => f.Id))),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Documents.Select(x => x.Id);
        }

        public virtual async Task<IEnumerable<string>> GetRunnableInstances(DateTime asAt, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();
            var now = asAt.ToUniversalTime().Ticks;

            var terms = new List<Func<QueryContainerDescriptor<WorkflowInstance>, QueryContainer>>();

            terms.Add(x => x.LongRange(t => t.Field(f => f.NextExecution).LessThanOrEquals(now)));
            terms.Add(x => x.Term(t => t.Field(f => f.Status).Value(WorkflowStatus.Runnable)));

            var response = await client.SearchAsync<WorkflowInstance>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.WorkflowInstanceIndex))
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.Includes(e => e.Field(f => f.Id))),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Documents.Select(x => x.Id);
        }

        public virtual async Task<EventSubscription> GetSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var id = Guid.Parse(eventSubscriptionId);

            var response = await client.GetAsync<EventSubscription>(
                id,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex)),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Source;
        }

        public virtual async Task<IEnumerable<EventSubscription>> GetSubscriptions(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();
            var now = asOf.ToUniversalTime();

            var terms = new List<Func<QueryContainerDescriptor<EventSubscription>, QueryContainer>>();

            terms.Add(x => x.Term(t => t.Field(f => f.EventName.Suffix("keyword")).Value(eventName)));
            terms.Add(x => x.Term(t => t.Field(f => f.EventKey.Suffix("keyword")).Value(eventKey)));
            terms.Add(x => x.DateRange(t => t.Field(f => f.SubscribeAsOf).LessThanOrEquals(now)));

            var response = await client.SearchAsync<EventSubscription>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex))
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.IncludeAll()),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Documents;
        }

        public virtual async Task<WorkflowInstance> GetWorkflowInstance(string Id, CancellationToken cancellationToken = default)
        {
            var workflowId = Guid.Parse(Id);
            var client = CreateClient();

            var response = await client.GetAsync<WorkflowInstance>(
                workflowId,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.WorkflowInstanceIndex)),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Source;
        }

        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take)
        {
            var client = CreateClient();

            var terms = new List<Func<QueryContainerDescriptor<WorkflowInstance>, QueryContainer>>();

            if (status.HasValue)
            {
                terms.Add(x => x.Term(t => t.Field(f => f.Status).Value(status.Value)));
            }
            if (!type.IsNullOrWhiteSpace())
            {
                terms.Add(x => x.Term(t => t.Field(f => f.WorkflowDefinitionId.Suffix("keyword")).Value(type)));
            }
            if (createdFrom.HasValue)
            {
                terms.Add(x => x.DateRange(t => t.Field(f => f.CreateTime).GreaterThanOrEquals(createdFrom.Value)));
            }
            if (createdTo.HasValue)
            {
                terms.Add(x => x.DateRange(t => t.Field(f => f.CreateTime).LessThanOrEquals(createdTo.Value)));
            }

            var response = await client.SearchAsync<WorkflowInstance>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.WorkflowInstanceIndex))
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.IncludeAll())
                    .Skip(skip)
                    .Take(take));

            CheckResponse(response);

            return response.Documents;
        }

        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();

            var response = await client.SearchAsync<WorkflowInstance>(
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.WorkflowInstanceIndex))
                    .Query(q =>
                        q.Bool(b =>
                            b.Should(s =>
                                s.Terms(t => t.Field(f => f.Id.Suffix("keyword")).Terms(ids)))))
                    .Source(s => s.IncludeAll()),
                ct: cancellationToken);

            CheckResponse(response);

            return response.Documents;
        }

        public virtual async Task MarkEventProcessed(string id, CancellationToken cancellationToken = default)
        {
            var eventId = Guid.Parse(id);
            var indexName = CreateIndex(PersistenceIndexConsts.EventIndex);

            var client = CreateClient();

            var response = await client.GetAsync<Event>(
                eventId,
                dsl => dsl.Index(indexName),
                ct: cancellationToken);

            CheckResponse(response);

            if (response.Found)
            {
                response.Source.IsProcessed = true;

                await client.UpdateAsync<Event>(
                    id,
                    dsl => dsl.Index(indexName)
                              .Doc(response.Source),
                    ct: cancellationToken);
            }
        }

        public virtual async Task MarkEventUnprocessed(string id, CancellationToken cancellationToken = default)
        {
            var eventId = Guid.Parse(id);
            var indexName = CreateIndex(PersistenceIndexConsts.EventIndex);

            var client = CreateClient();

            var response = await client.GetAsync<Event>(
                eventId,
                dsl => dsl.Index(indexName),
                ct: cancellationToken);

            CheckResponse(response);

            if (response.Found)
            {
                response.Source.IsProcessed = false;

                await client.UpdateAsync<Event>(
                    id,
                    dsl => dsl.Index(indexName)
                              .Doc(response.Source),
                    ct: cancellationToken);
            }
        }

        public virtual async Task PersistErrors(IEnumerable<ExecutionError> errors, CancellationToken cancellationToken = default)
        {
            var executionErrors = errors as ExecutionError[] ?? errors.ToArray();
            if (executionErrors.Any())
            {
                var client = CreateClient();

                var response = await client.IndexManyAsync(
                    errors,
                    CreateIndex(PersistenceIndexConsts.ExecutionErrorIndex),
                    cancellationToken: cancellationToken);

                CheckResponse(response);
            }
        }

        public virtual async Task PersistWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            var workflowId = Guid.Parse(workflow.Id);
            var indexName = CreateIndex(PersistenceIndexConsts.WorkflowInstanceIndex);

            var client = CreateClient();

            var response = await client.GetAsync<WorkflowInstance>(
                workflowId,
                dsl => dsl.Index(indexName),
                ct: cancellationToken);

            CheckResponse(response);

            await client.UpdateAsync<WorkflowInstance>(
                    workflowId,
                    dsl => dsl.Index(indexName)
                              .Doc(workflow),
                    ct: cancellationToken);
        }

        public virtual async Task ProcessCommands(DateTimeOffset asOf, Func<ScheduledCommand, Task> action, CancellationToken cancellationToken = default)
        {
            var client = CreateClient();
            var indexName = CreateIndex(PersistenceIndexConsts.ScheduledCommandIndex);

            var terms = new List<Func<QueryContainerDescriptor<PersistedScheduledCommand>, QueryContainer>>();

            terms.Add(x => x.LongRange(t => t.Field(f => f.ExecuteTime).LessThan(asOf.Ticks)));

            var response = await client.SearchAsync<PersistedScheduledCommand>(
                dsl => dsl.Index(indexName)
                    .Query(q => q.Bool(b => b.Filter(terms)))
                    .Source(s => s.IncludeAll()),
                ct: cancellationToken);

            CheckResponse(response);

            foreach (var command in response.Documents)
            {
                try
                {
                    await action(command.ToScheduledCommand());

                    await client.DeleteAsync<PersistedScheduledCommand>(
                        command.Id,
                        dsl => dsl.Index(indexName),
                        ct: cancellationToken);
                }
                catch (Exception)
                {
                    //TODO: add logger
                }
            }
        }

        public virtual async Task ScheduleCommand(ScheduledCommand command)
        {
            var persistedCommand = new PersistedScheduledCommand(
                _guidGenerator.Create(),
                command);

            var client = CreateClient();

            var response = await client.IndexAsync(
                persistedCommand,
                dsl => dsl
                    .Index(CreateIndex(PersistenceIndexConsts.ScheduledCommandIndex))
                    .Id(persistedCommand.Id));

            CheckResponse(response);
        }

        public virtual async Task<bool> SetSubscriptionToken(string eventSubscriptionId, string token, string workerId, DateTime expiry, CancellationToken cancellationToken = default)
        {
            var id = Guid.Parse(eventSubscriptionId);
            var indexName = CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex);

            var client = CreateClient();

            var response = await client.GetAsync<EventSubscription>(
                id,
                dsl => dsl.Index(indexName),
                ct: cancellationToken);

            CheckResponse(response);

            if (response.Found)
            {
                response.Source.ExternalToken = token;
                response.Source.ExternalWorkerId = workerId;
                response.Source.ExternalTokenExpiry = expiry;

                var uptResponse = await client.UpdateAsync<EventSubscription>(
                    id,
                    dsl => dsl.Index(indexName)
                              .Doc(response.Source),
                    ct: cancellationToken);

                return uptResponse.Result == Result.Updated;
            }

            return false;
        }

        public virtual async Task TerminateSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default)
        {
            var id = Guid.Parse(eventSubscriptionId);

            var client = CreateClient();

            var response = await client.DeleteAsync<EventSubscription>(
                id,
                dsl => dsl.Index(CreateIndex(PersistenceIndexConsts.EventSubscriptionIndex)),
                ct: cancellationToken);

            CheckResponse(response);
        }

        private IElasticClient CreateClient()
        {
            return _elasticsearchClientFactory.Create();
        }

        private string CreateIndex(string index)
        {
            return _indexNameNormalizer.NormalizeIndex(index);
        }

        private void CheckResponse(IResponse response)
        {
            if (!response.ApiCall.Success)
            {
                _logger.LogError(default(EventId), response.ApiCall.OriginalException, $"ES Operation Failed");
                throw new AbpException($"ES Operation Failed", response.ApiCall.OriginalException);
            }

            if (!response.IsValid)
            {
                _logger.LogWarning("ES Request Valid Error: {0}", response.DebugInformation);
                throw new InvalidOperationException(response.DebugInformation, response.OriginalException);
            }
        }
    }
}
