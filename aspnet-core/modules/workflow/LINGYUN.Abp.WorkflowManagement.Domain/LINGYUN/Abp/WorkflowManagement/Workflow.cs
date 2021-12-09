using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WorkflowManagement
{
    /// <summary>
    /// 流程定义
    /// </summary>
    public class Workflow : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnabled { get; protected set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public virtual int Version { get; protected set; }

        protected Workflow()
        {
        }

        public Workflow(
            Guid id,
            string displayName,
            string description = "",
            int version = 1) : base(id)
        {
        }
    }
}
