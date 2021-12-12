using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowManagement
{
    public abstract class Step : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid WorkflowId { get; protected set; }
        public virtual string Name { get; protected set;}
        public virtual string StepType { get; protected set; }
        public virtual string CancelCondition { get; set; }
        public virtual WorkflowErrorHandling? ErrorBehavior { get; protected set; }
        public virtual TimeSpan? RetryInterval { get; set; }
        public virtual bool Saga { get; set; }
        public virtual Guid? ParentId { get; protected set; }
        public virtual ExtraPropertyDictionary Inputs { get; set; }
        public virtual ExtraPropertyDictionary Outputs { get; set; }
        public virtual ExtraPropertyDictionary SelectNextStep { get; set; }
        protected Step() 
        {
            Inputs = new ExtraPropertyDictionary();
            Outputs = new ExtraPropertyDictionary();
            SelectNextStep = new ExtraPropertyDictionary();
        }

        protected Step(
            Guid id,
            Guid workflowId,
            string name,
            string stepType,
            string cancelCondition,
            WorkflowErrorHandling? errorBehavior = null,
            TimeSpan? retryInterval = null,
            bool saga = false,
            Guid? parentId = null,
            Guid? tenantId = null) : base(id)
        {
            Name = name;
            WorkflowId = workflowId;
            StepType = stepType;
            CancelCondition = cancelCondition;
            ErrorBehavior = errorBehavior;
            RetryInterval = retryInterval;
            Saga = saga;
            ParentId = parentId;
            TenantId = tenantId;

            Inputs = new ExtraPropertyDictionary();
            Outputs = new ExtraPropertyDictionary();
            SelectNextStep = new ExtraPropertyDictionary();
        }
    }
}
