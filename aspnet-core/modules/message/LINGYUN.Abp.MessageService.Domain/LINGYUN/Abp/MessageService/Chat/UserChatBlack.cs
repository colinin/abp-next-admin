using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    /// <summary>
    /// 用户黑名单
    /// </summary>
    public class UserChatBlack : CreationAuditedEntity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual Guid UserId { get; protected set; }
        /// <summary>
        /// 拉黑的用户
        /// </summary>
        public virtual Guid ShieldUserId { get; protected set; }
        protected UserChatBlack() { }
        public UserChatBlack(Guid userId, Guid shieldUserId, Guid? tenantId)
        {
            UserId = userId;
            ShieldUserId = shieldUserId;
            TenantId = tenantId;
        }
    }
}
