using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    internal static class ExtensionMethods
    {
        private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        internal static PersistedWorkflow ToPersistable(
            this WorkflowInstance instance,
            IGuidGenerator generator,
            ICurrentTenant currentTenant,
            PersistedWorkflow persistable = null)
        {
            if (persistable == null)
            {
                persistable = new PersistedWorkflow(
                    generator.Create(),
                    instance.CreateTime,
                    instance.WorkflowDefinitionId,
                    JsonConvert.SerializeObject(instance.Data, SerializerSettings),
                    instance.Version,
                    instance.Description,
                    instance.Reference,
                    instance.Status,
                    instance.NextExecution,
                    instance.CompleteTime,
                    currentTenant.Id);
            }
            else
            {
                persistable.Data = JsonConvert.SerializeObject(instance.Data, SerializerSettings);
                persistable.Description = instance.Description;
                persistable.Reference = instance.Reference;
                persistable.NextExecution = instance.NextExecution;
                persistable.Version = instance.Version;
                persistable.WorkflowDefinitionId = instance.WorkflowDefinitionId;
                persistable.Status = instance.Status;
                persistable.CreationTime = instance.CreateTime;
                persistable.CompleteTime = instance.CompleteTime;
            }

            foreach (var ep in instance.ExecutionPointers)
            {
                var epId = ep.Id.IsNullOrWhiteSpace() ? Guid.Empty : Guid.Parse(ep.Id);
                var persistedEP = persistable.FindPointer(epId);

                if (persistedEP == null)
                {
                    persistedEP = new PersistedExecutionPointer(
                        generator.Create(),
                        persistable.Id,
                        ep.StepId,
                        ep.StepName,
                        ep.Active,
                        JsonConvert.SerializeObject(ep.PersistenceData, SerializerSettings),
                        ep.EventName,
                        ep.EventKey,
                        ep.EventPublished,
                        JsonConvert.SerializeObject(ep.EventData, SerializerSettings),
                        ep.RetryCount,
                        ep.Children.JoinAsString(";"),
                        JsonConvert.SerializeObject(ep.ContextItem, SerializerSettings),
                        ep.PredecessorId,
                        JsonConvert.SerializeObject(ep.Outcome, SerializerSettings),
                        ep.Scope.JoinAsString(";"),
                        ep.Status,
                        ep.SleepUntil,
                        ep.StartTime,
                        ep.EndTime,
                        currentTenant.Id);

                    persistable.AddPointer(persistedEP);
                }
                else
                {
                    persistedEP.StepId = ep.StepId;
                    persistedEP.Active = ep.Active;
                    persistedEP.SleepUntil = ep.SleepUntil;
                    persistedEP.PersistenceData = JsonConvert.SerializeObject(ep.PersistenceData, SerializerSettings);
                    persistedEP.StartTime = ep.StartTime;
                    persistedEP.EndTime = ep.EndTime;
                    persistedEP.StepName = ep.StepName;
                    persistedEP.RetryCount = ep.RetryCount;
                    persistedEP.PredecessorId = ep.PredecessorId;
                    persistedEP.ContextItem = JsonConvert.SerializeObject(ep.ContextItem, SerializerSettings);
                    persistedEP.Children = ep.Children.JoinAsString(";");
                    persistedEP.EventName = ep.EventName;
                    persistedEP.EventKey = ep.EventKey;
                    persistedEP.EventPublished = ep.EventPublished;
                    persistedEP.EventData = JsonConvert.SerializeObject(ep.EventData, SerializerSettings);
                    persistedEP.Outcome = JsonConvert.SerializeObject(ep.Outcome, SerializerSettings);
                    persistedEP.Status = ep.Status;
                    persistedEP.Scope = ep.Scope.JoinAsString(";");
                }

                foreach (var attr in ep.ExtensionAttributes)
                {
                    var persistedAttr = persistedEP.FindAttribute(attr.Key);
                    if (persistedAttr == null)
                    {
                        persistedEP.AddAttribute(attr.Key, JsonConvert.SerializeObject(attr.Value, SerializerSettings));
                    }
                    else
                    {
                        persistedAttr.Key = attr.Key;
                        persistedAttr.Value = JsonConvert.SerializeObject(attr.Value, SerializerSettings);
                    }
                }
            }

            return persistable;
        }

        internal static PersistedExecutionError ToPersistable(
            this ExecutionError instance,
            ICurrentTenant currentTenant)
        {
            var result = new PersistedExecutionError(
                Guid.Parse(instance.WorkflowId),
                Guid.Parse(instance.ExecutionPointerId),
                instance.ErrorTime,
                instance.Message,
                currentTenant.Id);

            return result;
        }

        internal static PersistedSubscription ToPersistable(
            this EventSubscription instance,
            IGuidGenerator generator,
            ICurrentTenant currentTenant)
        {
            PersistedSubscription result = new PersistedSubscription(
                generator.Create(),
                Guid.Parse(instance.WorkflowId),
                instance.StepId,
                Guid.Parse(instance.ExecutionPointerId),
                instance.EventName,
                instance.EventKey,
                DateTime.SpecifyKind(instance.SubscribeAsOf, DateTimeKind.Utc),
                JsonConvert.SerializeObject(instance.SubscriptionData, SerializerSettings),
                instance.ExternalToken,
                instance.ExternalWorkerId,
                instance.ExternalTokenExpiry,
                currentTenant.Id);
            return result;
        }

        internal static PersistedEvent ToPersistable(
            this Event instance,
            IGuidGenerator generator,
            ICurrentTenant currentTenant)
        {
            PersistedEvent result = new PersistedEvent(
                generator.Create(),
                instance.EventName,
                instance.EventKey,
                JsonConvert.SerializeObject(instance.EventData, SerializerSettings),
                DateTime.SpecifyKind(instance.EventTime, DateTimeKind.Utc),
                currentTenant.Id);

            return result;
        }

        internal static PersistedScheduledCommand ToPersistable(
            this ScheduledCommand instance,
            ICurrentTenant currentTenant)
        {
            var result = new PersistedScheduledCommand(
                instance.CommandName,
                instance.Data,
                instance.ExecuteTime,
                currentTenant.Id);

            return result;
        }

        internal static WorkflowInstance ToWorkflowInstance(this PersistedWorkflow instance)
        {
            WorkflowInstance result = new WorkflowInstance();
            result.Data = JsonConvert.DeserializeObject(instance.Data, SerializerSettings);
            result.Description = instance.Description;
            result.Reference = instance.Reference;
            result.Id = instance.Id.ToString();
            result.NextExecution = instance.NextExecution;
            result.Version = instance.Version;
            result.WorkflowDefinitionId = instance.WorkflowDefinitionId;
            result.Status = instance.Status;
            result.CreateTime = DateTime.SpecifyKind(instance.CreationTime, DateTimeKind.Utc);
            if (instance.CompleteTime.HasValue)
                result.CompleteTime = DateTime.SpecifyKind(instance.CompleteTime.Value, DateTimeKind.Utc);

            result.ExecutionPointers = new ExecutionPointerCollection(instance.ExecutionPointers.Count + 8);

            foreach (var ep in instance.ExecutionPointers)
            {
                var pointer = new ExecutionPointer();

                pointer.Id = ep.Id.ToString();
                pointer.StepId = ep.StepId;
                pointer.Active = ep.Active;

                if (ep.SleepUntil.HasValue)
                    pointer.SleepUntil = DateTime.SpecifyKind(ep.SleepUntil.Value, DateTimeKind.Utc);

                pointer.PersistenceData = JsonConvert.DeserializeObject(ep.PersistenceData ?? string.Empty, SerializerSettings);

                if (ep.StartTime.HasValue)
                    pointer.StartTime = DateTime.SpecifyKind(ep.StartTime.Value, DateTimeKind.Utc);

                if (ep.EndTime.HasValue)
                    pointer.EndTime = DateTime.SpecifyKind(ep.EndTime.Value, DateTimeKind.Utc);

                pointer.StepName = ep.StepName;

                pointer.RetryCount = ep.RetryCount;
                pointer.PredecessorId = ep.PredecessorId;
                pointer.ContextItem = JsonConvert.DeserializeObject(ep.ContextItem ?? string.Empty, SerializerSettings);

                if (!string.IsNullOrEmpty(ep.Children))
                    pointer.Children = ep.Children.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                pointer.EventName = ep.EventName;
                pointer.EventKey = ep.EventKey;
                pointer.EventPublished = ep.EventPublished;
                pointer.EventData = JsonConvert.DeserializeObject(ep.EventData ?? string.Empty, SerializerSettings);
                pointer.Outcome = JsonConvert.DeserializeObject(ep.Outcome ?? string.Empty, SerializerSettings);
                pointer.Status = ep.Status;

                if (!string.IsNullOrEmpty(ep.Scope))
                    pointer.Scope = new List<string>(ep.Scope.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                foreach (var attr in ep.ExtensionAttributes)
                {
                    pointer.ExtensionAttributes[attr.Key] = JsonConvert.DeserializeObject(attr.Value, SerializerSettings);
                }

                result.ExecutionPointers.Add(pointer);
            }

            return result;
        }

        internal static EventSubscription ToEventSubscription(this PersistedSubscription instance)
        {
            EventSubscription result = new EventSubscription();
            result.Id = instance.Id.ToString();
            result.EventKey = instance.EventKey;
            result.EventName = instance.EventName;
            result.StepId = instance.StepId;
            result.ExecutionPointerId = instance.ExecutionPointerId.ToString();
            result.WorkflowId = instance.WorkflowId.ToString();
            result.SubscribeAsOf = DateTime.SpecifyKind(instance.SubscribeAsOf, DateTimeKind.Utc);
            result.SubscriptionData = JsonConvert.DeserializeObject(instance.SubscriptionData, SerializerSettings);
            result.ExternalToken = instance.ExternalToken;
            result.ExternalTokenExpiry = instance.ExternalTokenExpiry;
            result.ExternalWorkerId = instance.ExternalWorkerId;

            return result;
        }

        internal static Event ToEvent(this PersistedEvent instance)
        {
            Event result = new Event();
            result.Id = instance.Id.ToString();
            result.EventKey = instance.EventKey;
            result.EventName = instance.EventName;
            result.EventTime = DateTime.SpecifyKind(instance.CreationTime, DateTimeKind.Utc);
            result.IsProcessed = instance.IsProcessed;
            result.EventData = JsonConvert.DeserializeObject(instance.EventData, SerializerSettings);

            return result;
        }

        internal static ScheduledCommand ToScheduledCommand(this PersistedScheduledCommand instance)
        {
            var result = new ScheduledCommand();
            result.CommandName = instance.CommandName;
            result.Data = instance.Data;
            result.ExecuteTime = instance.ExecuteTime;

            return result;
        }
    }
}
