using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public static class WorkflowExtensions
    {
        public static Event ToEvent(this WorkflowEvent workflowEvent)
        {
            return new Event
            {
                Id = workflowEvent.Id.ToString(),
                EventName = workflowEvent.EventName,
                EventKey = workflowEvent.EventKey,
                EventTime = workflowEvent.CreationTime.ToUtcDateTime(),
                IsProcessed = workflowEvent.IsProcessed,
                EventData = workflowEvent.EventData.DeserializeObject()
            };
        }

        public static EventSubscription ToEventSubscription(this WorkflowEventSubscription workflowEventSubscription)
        {
            return new EventSubscription
            {
                Id = workflowEventSubscription.Id.ToString(),
                StepId = workflowEventSubscription.StepId,
                SubscribeAsOf = workflowEventSubscription.SubscribeAsOf.ToUtcDateTime(),
                SubscriptionData = workflowEventSubscription.SubscriptionData.DeserializeObject(),
                EventKey = workflowEventSubscription.EventKey,
                EventName = workflowEventSubscription.EventName,
                ExecutionPointerId = workflowEventSubscription.ExecutionPointerId.ToString(),
                ExternalWorkerId = workflowEventSubscription.ExternalWorkerId,
                ExternalToken = workflowEventSubscription.ExternalToken,
                ExternalTokenExpiry = workflowEventSubscription.ExternalTokenExpiry.ToNullableUtcDateTime(),
                WorkflowId = workflowEventSubscription.WorkflowId.ToString()
            };
        }

        public static WorkflowInstance ToWorkflowInstance(this Workflow workflow)
        {
            return new WorkflowInstance
            {
                Id = workflow.Id.ToString(),
                WorkflowDefinitionId = workflow.WorkflowDefinitionId,
                CompleteTime = workflow.CompleteTime.ToNullableUtcDateTime(),
                CreateTime = workflow.CreationTime.ToUtcDateTime(),
                Data = workflow.Data.SerializeObject(),
                Status = workflow.Status,
                Description = workflow.Description,
                NextExecution = workflow.NextExecution,
                Reference = workflow.Reference,
                Version = workflow.Version,
                ExecutionPointers = new ExecutionPointerCollection(
                    workflow.ExecutionPointers
                        .Select(pointer => pointer.ToExecutionPointer())
                        .ToList())
            };
        }

        public static ExecutionPointer ToExecutionPointer(this WorkflowExecutionPointer pointer)
        {
            return new ExecutionPointer
            {
                Id = pointer.Id.ToString(),
                EventData = pointer.EventData.DeserializeObject(),
                EventKey = pointer.StepName,
                EventName = pointer.EventName,
                EventPublished = pointer.EventPublished,
                ExtensionAttributes = pointer.ExtensionAttributes.ToExtensionAttributes(),
                Active = pointer.Active,
                Children = pointer.Children.Split(';').ToList(),
                ContextItem = pointer.ContextItem.DeserializeObject(),
                Scope = pointer.Scope.Split(';').ToList(),
                Outcome = pointer.Outcome.DeserializeObject(),
                PersistenceData = pointer.PersistenceData.DeserializeObject(),
                PredecessorId = pointer.PredecessorId,
                RetryCount = pointer.RetryCount,
                Status = pointer.Status,
                StepId = pointer.StepId,
                StepName = pointer.StepName,
                EndTime = pointer.EndTime.ToNullableUtcDateTime(),
                StartTime = pointer.StartTime.ToNullableUtcDateTime(),
                SleepUntil = pointer.SleepUntil.ToNullableUtcDateTime(),
            };
        }

        public static ScheduledCommand ToScheduledCommand(
            this WorkflowScheduledCommand command)
        {
            return new ScheduledCommand
            {
                CommandName = command.CommandName,
                Data = command.Data,
                ExecuteTime = command.ExecuteTime
            };
        }

        public static Dictionary<string, object> ToExtensionAttributes(
            this ICollection<WorkflowExtensionAttribute> attributes)
        {
            var attrDic = new Dictionary<string, object>();

            foreach (var attr in attributes)
            {
                attrDic.Add(attr.Key, attr.Value.DeserializeObject());
            }

            return attrDic;
        }
    }
}
