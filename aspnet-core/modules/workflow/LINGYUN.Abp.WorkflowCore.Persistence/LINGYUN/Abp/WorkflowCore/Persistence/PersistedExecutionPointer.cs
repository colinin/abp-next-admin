using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class PersistedExecutionPointer : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid WorkflowId { get; protected set; }

        public virtual PersistedWorkflow Workflow { get; protected set; }

        public virtual int StepId { get; set; }

        public virtual bool Active { get; set; }

        public virtual DateTime? SleepUntil { get; set; }

        public virtual string PersistenceData { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? EndTime { get; set; }

        public virtual string EventName { get; set; }

        public virtual string EventKey { get; set; }

        public virtual bool EventPublished { get; set; }

        public virtual string EventData { get; set; }

        public virtual string StepName { get; set; }

        public virtual int RetryCount { get; set; }

        public virtual string Children { get; set; }

        public virtual string ContextItem { get; set; }

        public virtual string PredecessorId { get; set; }

        public virtual string Outcome { get; set; }

        public virtual PointerStatus Status { get; set; }

        public virtual string Scope { get; set; }

        public virtual ICollection<PersistedExtensionAttribute> ExtensionAttributes { get; protected set; }

        protected PersistedExecutionPointer()
        {
            ExtensionAttributes = new Collection<PersistedExtensionAttribute>();
        }

        public PersistedExecutionPointer(
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

            ExtensionAttributes = new Collection<PersistedExtensionAttribute>();
        }

        public PersistedExtensionAttribute AddAttribute(string key, string value)
        {
            var attr = new PersistedExtensionAttribute(Id, key, value);
            ExtensionAttributes.Add(attr);

            return attr;
        }

        public PersistedExtensionAttribute FindAttribute(string key)
        {
            return ExtensionAttributes.FirstOrDefault(x => x.Key.Equals(key));
        }
    }
}
