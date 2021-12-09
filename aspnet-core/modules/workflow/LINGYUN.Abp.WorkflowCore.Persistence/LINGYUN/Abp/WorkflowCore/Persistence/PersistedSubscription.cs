using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class PersistedSubscription : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        public virtual Guid WorkflowId { get; protected set; }

        public virtual int StepId { get; protected set; }

        public virtual Guid ExecutionPointerId { get; protected set; }

        public virtual string EventName { get; protected set; }

        public virtual string EventKey { get; protected set; }

        public virtual DateTime SubscribeAsOf { get; protected set; }

        public virtual string SubscriptionData { get; protected set; }

        public virtual string ExternalToken { get; set; }

        public virtual string ExternalWorkerId { get; set; }

        public virtual DateTime? ExternalTokenExpiry { get; set; }

        protected PersistedSubscription()
        {
        }

        public PersistedSubscription(
            Guid id,
            Guid workflowId,
            int stepId,
            Guid pointerId,
            string eventName,
            string eventKey,
            DateTime subscribeAsOf,
            string subscriptionData,
            string externalToken,
            string externalWorkerId,
            DateTime? externalTokenExpiry = null,
            Guid? tenantId = null) : base(id)
        {
            WorkflowId = workflowId;
            StepId = stepId;
            ExecutionPointerId = pointerId;
            EventName = eventName;
            EventKey = eventKey;
            SubscribeAsOf = subscribeAsOf;
            SubscriptionData = subscriptionData;
            ExternalToken = externalToken;
            ExternalWorkerId = externalWorkerId;
            ExternalTokenExpiry = externalTokenExpiry;

            TenantId = tenantId;
        }

        public void SetSubscriptionToken(string token, string workerId, DateTime? expiry = null)
        {
            ExternalToken = token;
            ExternalWorkerId = workerId;
            ExternalTokenExpiry = expiry;
        }
    }
}
