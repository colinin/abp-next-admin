using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    /// <summary>
    /// 用户特别关注
    /// </summary>
    public class UserSpecialFocus : CreationAuditedEntity<long>, IMultiTenant
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
        /// 关注的用户
        /// </summary>
        public virtual Guid FocusUserId { get; protected set; }
        protected UserSpecialFocus() { }
        public UserSpecialFocus(Guid userId, Guid focusUserId, Guid? tenantId)
        {
            UserId = userId;
            FocusUserId = focusUserId;
            TenantId = tenantId;
        }
    }
}
