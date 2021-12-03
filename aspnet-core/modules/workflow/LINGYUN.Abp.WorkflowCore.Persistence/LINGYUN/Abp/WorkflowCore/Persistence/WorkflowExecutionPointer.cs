using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class WorkflowExecutionPointer : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid WorkflowId { get; protected set; }

        public virtual Workflow Workflow { get; protected set; }

        public virtual int StepId { get; protected set; }

        public virtual bool Active { get; protected set; }

        public virtual DateTime? SleepUntil { get; protected set; }

        public virtual string PersistenceData { get; protected set; }

        public virtual DateTime? StartTime { get; protected set; }

        public virtual DateTime? EndTime { get; protected set; }

        public virtual string EventName { get; protected set; }

        public virtual string EventKey { get; protected set; }

        public virtual bool EventPublished { get; protected set; }

        public virtual string EventData { get; protected set; }

        public virtual string StepName { get; protected set; }

        public virtual int RetryCount { get; protected set; }

        public virtual string Children { get; protected set; }

        public virtual string ContextItem { get; protected set; }

        public virtual string PredecessorId { get; protected set; }

        public virtual string Outcome { get; protected set; }

        public virtual PointerStatus Status { get; protected set; }

        public virtual string Scope { get; protected set; }

        public virtual ICollection<WorkflowExtensionAttribute> ExtensionAttributes { get; protected set; }

        protected WorkflowExecutionPointer()
        {
            ExtensionAttributes = new Collection<WorkflowExtensionAttribute>();
        }

        public WorkflowExecutionPointer(
            Guid id,
            Guid workflowId,
            int stepId,
            string stepName,
            bool active,
            string persistenceData,
            string eventName,
            string eventKey,
            bool eventPublished,
            string eventData,
            int retryCount,
            string children,
            string contextItem,
            string predecessorId,
            string outcome,
            string scope,
            PointerStatus status = PointerStatus.Legacy,
            DateTime? sleepUntil = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            Guid? tenantId = null) : base(id)
        {
            WorkflowId = workflowId;
            StepId = stepId;
            StepName = stepName;
            Active = active;
            PersistenceData = persistenceData;
            EventName = eventName;
            EventKey = eventKey;
            EventPublished = eventPublished;
            EventData = eventData;
            RetryCount = retryCount;
            Children = children;
            ContextItem = contextItem;
            PredecessorId = predecessorId;
            Outcome = outcome;
            Scope = scope;
            Status = status;
            SleepUntil = sleepUntil;
            StartTime = startTime;
            EndTime = endTime;

            TenantId = tenantId;

            ExtensionAttributes = new Collection<WorkflowExtensionAttribute>();
        }

        public void Update(ExecutionPointer pointer)
        {
            StepId = pointer.StepId;
            StepName = pointer.StepName;
            Active = pointer.Active;
            PersistenceData = JsonConvert.SerializeObject(pointer.PersistenceData);
            EventName = pointer.EventName;
            EventKey = pointer.EventKey;
            EventPublished = pointer.EventPublished;
            EventData = JsonConvert.SerializeObject(pointer.EventData);
            RetryCount = pointer.RetryCount;
            Children = pointer.Children.JoinAsString(";");
            ContextItem = JsonConvert.SerializeObject(pointer.ContextItem);
            PredecessorId = pointer.PredecessorId;
            Outcome = JsonConvert.SerializeObject(pointer.Outcome);
            Scope = pointer.Scope.JoinAsString(";");
            Status = pointer.Status;
            SleepUntil = pointer.SleepUntil;
            StartTime = pointer.StartTime;
            EndTime = pointer.EndTime;

            foreach (var attribute in pointer.ExtensionAttributes)
            {
                var findAttr = FindAttribute(attribute.Key);
                if (findAttr == null)
                {
                    findAttr = new WorkflowExtensionAttribute(Id, attribute.Key, attribute.Value.SerializeObject());

                }
                else
                {
                    findAttr.Key = attribute.Key;
                    findAttr.Value = attribute.Value.SerializeObject();
                }
            }
        }

        public WorkflowExtensionAttribute AddAttribute(string key, string value)
        {
            var attr = new WorkflowExtensionAttribute(Id, key, value);
            ExtensionAttributes.Add(attr);

            return attr;
        }

        public WorkflowExtensionAttribute FindAttribute(string key)
        {
            return ExtensionAttributes.FirstOrDefault(x => x.Key.Equals(key));
        }
    }
}
