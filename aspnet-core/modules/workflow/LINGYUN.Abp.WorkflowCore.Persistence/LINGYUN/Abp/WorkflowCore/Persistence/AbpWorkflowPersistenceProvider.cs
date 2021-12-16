using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class AbpWorkflowPersistenceProvider : IPersistenceProvider, IUnitOfWorkEnabled
    {
        public ILogger<AbpWorkflowPersistenceProvider> Logger { protected get; set; }

        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IWorkflowEventRepository _workflowEventRepository;
        private readonly IWorkflowExecutionErrorRepository _executionErrorRepository;
        private readonly IWorkflowEventSubscriptionRepository _subscriptionRepository;
        private readonly IWorkflowScheduledCommandRepository _scheduledCommandRepository;

        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        public bool SupportsScheduledCommands => true;

        public AbpWorkflowPersistenceProvider(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IWorkflowRepository workflowRepository,
            IWorkflowEventRepository workflowEventRepository,
            IWorkflowExecutionErrorRepository executionErrorRepository,
            IWorkflowEventSubscriptionRepository subscriptionRepository,
            IWorkflowScheduledCommandRepository scheduledCommandRepository)
        {
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _asyncQueryableExecuter = asyncQueryableExecuter;

            _workflowRepository = workflowRepository;
            _workflowEventRepository = workflowEventRepository;
            _executionErrorRepository = executionErrorRepository;
            _subscriptionRepository = subscriptionRepository;
            _scheduledCommandRepository = scheduledCommandRepository;

            Logger = NullLogger<AbpWorkflowPersistenceProvider>.Instance;
        }

        public virtual async Task ClearSubscriptionToken(
            string eventSubscriptionId, 
            string token, 
            CancellationToken cancellationToken = default)
        {
            var uid = Guid.Parse(eventSubscriptionId);
            var existingEntity = await _subscriptionRepository.GetAsync(uid, cancellationToken: cancellationToken);

            if (existingEntity.ExternalToken != token)
                throw new InvalidOperationException();

            existingEntity.SetSubscriptionToken(null, null, null);

            await _subscriptionRepository.UpdateAsync(existingEntity, cancellationToken: cancellationToken);
        }

        public virtual async Task<string> CreateEvent(
            Event newEvent, 
            CancellationToken cancellationToken = default)
        {
            var we = newEvent.ToPersistable(_guidGenerator, _currentTenant);

            await _workflowEventRepository.InsertAsync(we, cancellationToken: cancellationToken);

            newEvent.Id = we.Id.ToString();

            return newEvent.Id;
        }

        public virtual async Task<string> CreateEventSubscription(
            EventSubscription subscription,
            CancellationToken cancellationToken = default)
        {
            var wes = subscription.ToPersistable(_guidGenerator, _currentTenant);

            await _subscriptionRepository.InsertAsync(wes, cancellationToken: cancellationToken);

            subscription.Id = wes.Id.ToString();

            return subscription.Id;
        }

        public virtual async Task<string> CreateNewWorkflow(
            WorkflowInstance workflow, 
            CancellationToken cancellationToken = default)
        {
            var wf = workflow.ToPersistable(_guidGenerator, _currentTenant);

            await _workflowRepository.InsertAsync(wf, cancellationToken: cancellationToken);

            workflow.Id = wf.Id.ToString();

            return workflow.Id;
        }

        public void EnsureStoreExists()
        {
            // TODO:
        }

        public virtual async Task<Event> GetEvent(string id, CancellationToken cancellationToken = default)
        {
            var eventId = Guid.Parse(id);

            var workflowEvent = await _workflowEventRepository.GetAsync(eventId, cancellationToken: cancellationToken);

            return workflowEvent.ToEvent();
        }

        public virtual async Task<IEnumerable<string>> GetEvents(
            string eventName, 
            string eventKey, 
            DateTime asOf, 
            CancellationToken cancellationToken = default)
        {
            var queryable = await _workflowEventRepository.GetQueryableAsync();
            var workflowEventIds = await _asyncQueryableExecuter.ToListAsync(
                queryable.Where(x => x.EventName == eventName && x.EventKey == eventKey)
                         .Where(x => x.CreationTime >= asOf)
                         .Select(x => x.Id),
                cancellationToken);

            return workflowEventIds.Select(e => e.ToString());
        }

        public virtual async Task<EventSubscription> GetFirstOpenSubscription(
            string eventName, 
            string eventKey, 
            DateTime asOf, 
            CancellationToken cancellationToken = default)
        {
            var queryable = await _subscriptionRepository.GetQueryableAsync();
            var workflowEventSubscription = await _asyncQueryableExecuter.FirstOrDefaultAsync(
                queryable.Where(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= asOf && x.ExternalToken == null),
                cancellationToken);

            return workflowEventSubscription?.ToEventSubscription();
        }

        public virtual async Task<IEnumerable<string>> GetRunnableEvents(
            DateTime asAt, 
            CancellationToken cancellationToken = default)
        {
            var workflowEvents = await _workflowEventRepository.GetQueryableAsync();

            var now = asAt.ToUniversalTime();

            var workflowEventIdList = await _asyncQueryableExecuter.ToListAsync(
                workflowEvents.Where(x => !x.IsProcessed)
                              .Where(x => x.CreationTime <= now)
                              .Select(x => x.Id),
                cancellationToken);

            return workflowEventIdList.Select(e => e.ToString());
        }

        public virtual async Task<IEnumerable<string>> GetRunnableInstances(
            DateTime asAt, 
            CancellationToken cancellationToken = default)
        {
            var now = asAt.ToUniversalTime().Ticks;

            var workflows = await _workflowRepository.GetQueryableAsync();
            var workflowIdList = await _asyncQueryableExecuter.ToListAsync(
                workflows.Where(x => x.NextExecution.HasValue && (x.NextExecution <= now) && (x.Status == WorkflowStatus.Runnable))
                         .Select(x => x.Id),
                cancellationToken);

            return workflowIdList.Select(e => e.ToString());
        }

        public virtual async Task<EventSubscription> GetSubscription(
            string eventSubscriptionId, 
            CancellationToken cancellationToken = default)
        {
            var subscriptionId = Guid.Parse(eventSubscriptionId);
            var subscription = await _subscriptionRepository.FindAsync(subscriptionId, cancellationToken: cancellationToken);

            return subscription?.ToEventSubscription();
        }

        public virtual async Task<IEnumerable<EventSubscription>> GetSubscriptions(
            string eventName, 
            string eventKey, 
            DateTime asOf, 
            CancellationToken cancellationToken = default)
        {
            var now = asOf.ToUniversalTime();
            var subscriptions = await _subscriptionRepository.GetQueryableAsync();
            var eventSubscriptions = await _asyncQueryableExecuter.ToListAsync(
                subscriptions.Where(x => x.EventName == eventName && x.EventKey == eventKey && x.SubscribeAsOf <= now),
                cancellationToken);

            return eventSubscriptions.Select(x => x.ToEventSubscription());
        }

        public virtual async Task<WorkflowInstance> GetWorkflowInstance(
            string Id, 
            CancellationToken cancellationToken = default)
        {
            var workflowId = Guid.Parse(Id);
            var workflow = await _workflowRepository.FindAsync(
                workflowId, 
                includeDetails: true, 
                cancellationToken: cancellationToken);

            return workflow?.ToWorkflowInstance();
        }

        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(
            WorkflowStatus? status, 
            string type, 
            DateTime? createdFrom, 
            DateTime? createdTo, 
            int skip, 
            int take)
        {
            var workflows = await _workflowRepository.GetListAsync(status, type, createdFrom, createdTo, skip, take);

            return workflows.Select(x => x.ToWorkflowInstance());
        }

        public virtual async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(
            IEnumerable<string> ids,
            CancellationToken cancellationToken = default)
        {
            var workflowIds = ids.Select(id => Guid.Parse(id));

            var queryable = await _workflowRepository.GetQueryableAsync();
            var workflows = await _asyncQueryableExecuter.ToListAsync(
                queryable.Where(x => workflowIds.Contains(x.Id)),
                cancellationToken);

            return workflows.Select(x => x.ToWorkflowInstance());
        }

        public virtual async Task MarkEventProcessed(string id, CancellationToken cancellationToken = default)
        {
            var eventId = Guid.Parse(id);
            var workflowEvent = await _workflowEventRepository.GetAsync(eventId, cancellationToken: cancellationToken);

            workflowEvent.IsProcessed = true;

            await _workflowEventRepository.UpdateAsync(workflowEvent, cancellationToken: cancellationToken);
        }

        public virtual async Task MarkEventUnprocessed(string id, CancellationToken cancellationToken = default)
        {
            var eventId = Guid.Parse(id);
            var workflowEvent = await _workflowEventRepository.GetAsync(eventId, cancellationToken: cancellationToken);

            workflowEvent.IsProcessed = false;

            await _workflowEventRepository.UpdateAsync(workflowEvent, cancellationToken: cancellationToken);
        }

        public virtual async Task PersistErrors(IEnumerable<ExecutionError> errors, CancellationToken cancellationToken = default)
        {
            if (errors.Any())
            {
                var workflowExecutionErrors = errors.Select(x => x.ToPersistable(_currentTenant));

                await _executionErrorRepository.InsertManyAsync(workflowExecutionErrors, cancellationToken: cancellationToken);
            }
        }

        public virtual async Task PersistWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(workflow.Id, out Guid workflowId))
            {
                workflowId = _guidGenerator.Create();
            }

            var wf = await _workflowRepository.FindAsync(workflowId, includeDetails: true, cancellationToken: cancellationToken);
            if (wf == null)
            {
                wf = workflow.ToPersistable(_guidGenerator, _currentTenant);

                await _workflowRepository.InsertAsync(wf, cancellationToken: cancellationToken);
            }
            else
            {
                wf = workflow.ToPersistable(_guidGenerator, _currentTenant, wf);

                await _workflowRepository.UpdateAsync(wf, cancellationToken: cancellationToken);
            }
        }

        public virtual async Task ProcessCommands(
            DateTimeOffset asOf, 
            Func<ScheduledCommand, Task> action, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var quertable = await _scheduledCommandRepository.GetQueryableAsync();
                var commands = await _asyncQueryableExecuter.ToListAsync(
                    quertable.Where(x => x.ExecuteTime < asOf.UtcDateTime.Ticks),
                    cancellationToken);

                foreach (var command in commands)
                {
                    await action(command.ToScheduledCommand());
                }

                await _scheduledCommandRepository.DeleteManyAsync(commands, cancellationToken: cancellationToken);
            }
            catch(Exception ex)
            {
                // TODO
                Logger.LogWarning("Process Commands Error: {0}", ex.Message);
            }
        }

        public virtual async Task ScheduleCommand(ScheduledCommand command)
        {
            if (!await _scheduledCommandRepository.CheckExistsAsync(command.CommandName, command.Data))
            {
                var workflowCommand = command.ToPersistable(_currentTenant);

                await _scheduledCommandRepository.InsertAsync(workflowCommand);
            }
        }

        public virtual async Task<bool> SetSubscriptionToken(
            string eventSubscriptionId, 
            string token,
            string workerId,
            DateTime expiry,
            CancellationToken cancellationToken = default)
        {
            var uid = Guid.Parse(eventSubscriptionId);

            var existingEntity = await _subscriptionRepository.GetAsync(uid, cancellationToken: cancellationToken);

            existingEntity.SetSubscriptionToken(token, workerId, expiry);

            await _subscriptionRepository.UpdateAsync(existingEntity, cancellationToken: cancellationToken);

            return true;
        }

        public virtual async Task TerminateSubscription(
            string eventSubscriptionId, 
            CancellationToken cancellationToken = default)
        {
            var uid = Guid.Parse(eventSubscriptionId);

            var existingEntity = await _subscriptionRepository.GetAsync(uid, cancellationToken: cancellationToken);

            await _subscriptionRepository.DeleteAsync(existingEntity, cancellationToken: cancellationToken);
        }
    }
}
