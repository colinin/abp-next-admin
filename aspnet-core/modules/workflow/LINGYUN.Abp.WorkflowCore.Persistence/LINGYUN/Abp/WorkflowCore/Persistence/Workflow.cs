using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class Workflow : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string WorkflowDefinitionId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string Reference { get; protected set; }
        public virtual long? NextExecution { get; protected set; }
        public virtual WorkflowStatus Status { get; protected set; }
        public virtual string Data { get; protected set; }
        public virtual DateTime? CompleteTime { get; protected set; }
        public virtual ICollection<WorkflowExecutionPointer> ExecutionPointers { get; protected set; }

        protected Workflow()
        {
            ExecutionPointers = new Collection<WorkflowExecutionPointer>();
        }

        public Workflow(
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

            ExecutionPointers = new Collection<WorkflowExecutionPointer>();
        }

        public void AddPointer(WorkflowExecutionPointer pointer)
        {
            ExecutionPointers.Add(pointer);
        }

        public WorkflowExecutionPointer FindPointer(Guid id)
        {
            return ExecutionPointers.FirstOrDefault(point => point.Id.Equals(id));
        }

        public void Update(
            WorkflowInstance instance, 
            IGuidGenerator guidGenerator, 
            ICurrentTenant currentTenant)
        {
            Data = instance.Data.SerializeObject();
            CreationTime = instance.CreateTime;
            WorkflowDefinitionId = instance.WorkflowDefinitionId;
            Version = instance.Version;
            Description = instance.Description;
            Reference = instance.Reference;
            NextExecution = instance.NextExecution;
            Status = instance.Status;
            CompleteTime = instance.CompleteTime;

            foreach (var pointer in instance.ExecutionPointers)
            {
                if (!Guid.TryParse(pointer.Id, out Guid pointerId))
                {
                    pointerId = guidGenerator.Create();
                }

                var currentPointer = FindPointer(pointerId);
                if (currentPointer != null)
                {
                    currentPointer.Update(pointer);
                    continue;
                }

                AddPointer(pointer.ToWorkflowExecutionPointer(this, guidGenerator, currentTenant));
            }
        }
    }
}
