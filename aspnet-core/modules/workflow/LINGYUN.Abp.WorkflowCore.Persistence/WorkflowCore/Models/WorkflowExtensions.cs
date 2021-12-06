using LINGYUN.Abp.WorkflowCore.Persistence;
using System;
using System.Collections.Generic;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace WorkflowCore.Models
{
    public static class WorkflowExtensions
    {
        public static Workflow ToWorkflow(
            this WorkflowInstance instance,
            IGuidGenerator generator,
            ICurrentTenant currentTenant)
        {
            var workflow = new Workflow(
                generator.Create(),
                instance.CreateTime,
                instance.WorkflowDefinitionId,
                instance.Data.SerializeObject(handlingString: true),
                instance.Version,
                instance.Description,
                instance.Reference,
                instance.Status,
                instance.NextExecution,
                instance.CompleteTime,
                currentTenant.Id);

            foreach (var pointer in instance.ExecutionPointers)
            {
                var toPointer = pointer.ToWorkflowExecutionPointer(workflow, generator, currentTenant);
                workflow.AddPointer(toPointer);
            }

            return workflow;
        }

        public static WorkflowExecutionPointer ToWorkflowExecutionPointer(
            this ExecutionPointer executionPointer,
            Workflow workflow,
            IGuidGenerator generator,
            ICurrentTenant currentTenant)
        {
            var pointer = new WorkflowExecutionPointer(
                generator.Create(),
                workflow.Id,
                executionPointer.StepId,
                executionPointer.StepName,
                executionPointer.Active,
                executionPointer.PersistenceData.SerializeObject(handlingString: true),
                executionPointer.EventName,
                executionPointer.EventKey,
                executionPointer.EventPublished,
                executionPointer.EventData.SerializeObject(handlingString: true),
                executionPointer.RetryCount,
                executionPointer.Children.JoinAsString(";"),
                executionPointer.ContextItem.SerializeObject(handlingString: true),
                executionPointer.PredecessorId,
                executionPointer.Outcome.SerializeObject(handlingString: true),
                executionPointer.Scope.JoinAsString(";"),
                executionPointer.Status,
                executionPointer.SleepUntil,
                executionPointer.StartTime,
                executionPointer.EndTime,
                currentTenant.Id);

            foreach (var attribute in executionPointer.ExtensionAttributes)
            {
                pointer.AddAttribute(attribute.Key, attribute.Value.SerializeObject(handlingString: true));
            }

            executionPointer.Id = pointer.Id.ToString();

            return pointer;
        }

        public static WorkflowEvent ToWorkflowEvent(
            this Event @event,
            IGuidGenerator generator,
            ICurrentTenant currentTenant)
        {
            var we = new WorkflowEvent(
                generator.Create(),
                @event.EventName,
                @event.EventKey,
                @event.EventData.SerializeObject(handlingString: true),
                @event.EventTime,
                currentTenant.Id)
            {
                IsProcessed = @event.IsProcessed
            };

            return we;
        }

        public static WorkflowEventSubscription ToWorkflowEventSubscription(
            this EventSubscription subscription,
            IGuidGenerator generator,
            ICurrentTenant currentTenant)
        {
            return new WorkflowEventSubscription(
                generator.Create(),
                Guid.Parse(subscription.WorkflowId),
                subscription.StepId,
                Guid.Parse(subscription.ExecutionPointerId),
                subscription.EventName,
                subscription.EventKey,
                subscription.SubscribeAsOf,
                subscription.SubscriptionData.SerializeObject(handlingString: true),
                subscription.ExternalToken,
                subscription.ExternalWorkerId,
                subscription.ExternalTokenExpiry,
                currentTenant.Id);
        }

        public static WorkflowExecutionError ToWorkflowExecutionError(
            this ExecutionError executionError,
            ICurrentTenant currentTenant)
        {
            return new WorkflowExecutionError(
                Guid.Parse(executionError.WorkflowId),
                Guid.Parse(executionError.ExecutionPointerId),
                executionError.ErrorTime,
                executionError.Message,
                currentTenant.Id);
        }

        public static WorkflowScheduledCommand ToWorkflowScheduledCommand(
            this ScheduledCommand command,
            ICurrentTenant currentTenant)
        {
            return new WorkflowScheduledCommand(
                command.CommandName,
                command.Data,
                command.ExecuteTime,
                currentTenant.Id);
        }
    }
}
