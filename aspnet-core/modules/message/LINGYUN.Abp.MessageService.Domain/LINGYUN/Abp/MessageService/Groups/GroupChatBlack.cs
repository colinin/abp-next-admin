using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Groups
{
    /// <summary>
    /// 用户黑名单
    /// </summary>
    public class GroupChatBlack : CreationAuditedEntity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 群组标识
        /// </summary>
        public virtual long GroupId { get; protected set; }
        /// <summary>
        /// 拉黑的用户
        /// </summary>
        public virtual Guid ShieldUserId { get; protected set; }
        protected GroupChatBlack() { }
        public GroupChatBlack(long groupId, Guid shieldUserId, Guid? tenantId)
        {
            GroupId = groupId;
            ShieldUserId = shieldUserId;
            TenantId = tenantId;
        }
    }
}
