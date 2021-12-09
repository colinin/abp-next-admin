using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public class PersistedEvent : Entity<Guid>, IMultiTenant, IHasCreationTime
    {
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string EventName { get; protected set; }
        /// <summary>
        /// Key
        /// </summary>
        public virtual string EventKey { get; protected set; }
        /// <summary>
        /// 数据
        /// </summary>
        public virtual string EventData { get; protected set; }
        /// <summary>
        /// 是否已处理
        /// </summary>
        public virtual bool IsProcessed { get; set; }
        /// <summary>
        /// 建立时间
        /// </summary>
        public virtual DateTime CreationTime { get; protected set; }
        protected PersistedEvent() { }
        public PersistedEvent(
            Guid id,
            string name,
            string key,
            string data,
            DateTime creationTime,
            Guid? tenantId = null) : base(id)
        {
            EventName = name;
            EventKey = key;
            EventData = data;
            CreationTime = creationTime;
            TenantId = tenantId;
        }
    }
}
