using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserChatFriend : CreationAuditedEntity<long>, IMultiTenant
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
        /// 好友标识
        /// </summary>
        public virtual Guid FrientId { get; protected set; }
        /// <summary>
        /// 已添加黑名单
        /// </summary>
        public virtual bool Black { get; set; }
        /// <summary>
        /// 消息免打扰
        /// </summary>
        public virtual bool DontDisturb { get; set; }
        /// <summary>
        /// 特别关注
        /// </summary>
        public virtual bool SpecialFocus { get; set; }
        /// <summary>
        /// 备注名称
        /// </summary>
        public virtual string RemarkName { get; set; }

        protected UserChatFriend()
        {
        }

        public UserChatFriend(
            Guid userId,
            Guid friendId,
            string remarkName = "",
            Guid? tenantId = null)
        {
            UserId = userId;
            FrientId = friendId;
            RemarkName = remarkName;
            TenantId = tenantId;
        }
    }
}
