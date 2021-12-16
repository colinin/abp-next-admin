using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class PersistedWorkflow : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string WorkflowDefinitionId { get; set; }
        public virtual int Version { get; set; }
        public virtual string Description { get; set; }
        public virtual string Reference { get; set; }
        public virtual long? NextExecution { get; set; }
        public virtual WorkflowStatus Status { get; set; }
        public virtual string Data { get; set; }
        public virtual DateTime? CompleteTime { get; set; }
        public virtual ICollection<PersistedExecutionPointer> ExecutionPointers { get; protected set; }

        protected PersistedWorkflow()
        {
            ExecutionPointers = new Collection<PersistedExecutionPointer>();
        }

        public PersistedWorkflow(
            Guid id,
            DateTime creationTime,
            string defintionId,
            string data,
            int version,
            string description,
            string reference,
            WorkflowStatus status,
            long? nextExecution = null,
            DateTime? completeTime = null,
            Guid? tenantId = null) : base(id)
        {
            Data = data;
            CreationTime = creationTime;
            WorkflowDefinitionId = defintionId;
            Version = version;
            Description = description;
            Reference = reference;
            NextExecution = nextExecution;
            Status = status;
            CompleteTime = completeTime;
            TenantId = tenantId;

            ExecutionPointers = new Collection<PersistedExecutionPointer>();
        }

        public void AddPointer([NotNull] PersistedExecutionPointer pointer)
        {
            Check.NotNull(pointer, nameof(pointer));

            ExecutionPointers.Add(pointer);
        }

        public PersistedExecutionPointer FindPointer(Guid id)
        {
            return ExecutionPointers.FirstOrDefault(point => point.Id.Equals(id));
        }

        public void RemovePointers([NotNull] IEnumerable<PersistedExecutionPointer> pointers)
        {
            Check.NotNull(pointers, nameof(pointers));

            foreach (var pointer in pointers)
            {
                RemovePointer(pointer);
            }
        }

        public virtual void RemovePointer([NotNull] PersistedExecutionPointer pointer)
        {
            Check.NotNull(pointer, nameof(pointer));

            ExecutionPointers.RemoveAll(c => c.Id == pointer.Id);
        }
    }
}
