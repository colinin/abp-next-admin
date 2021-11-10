using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Groups
{
    /// <summary>
    /// 用户群组
    /// </summary>
    public class UserChatGroup : CreationAuditedEntity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual long GroupId { get; protected set; }
        protected UserChatGroup() { }
        public UserChatGroup(long groupId, Guid userId, Guid acceptUserId, Guid? tenantId = null)
        {
            UserId = userId;
            GroupId = groupId;
            CreatorId = acceptUserId;
            TenantId = tenantId;
        }
    }
}
