using LINGYUN.Abp.IM.Contract;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserChatFriend : CreationAuditedAggregateRoot<long>, IMultiTenant
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

        public virtual UserFriendStatus Status { get; protected set; }

        protected UserChatFriend()
        {
        }

        public UserChatFriend(
            Guid userId,
            Guid friendId,
            string remarkName = "",
            UserFriendStatus status = UserFriendStatus.NeedValidation,
            Guid? tenantId = null)
        {
            UserId = userId;
            FrientId = friendId;
            RemarkName = remarkName;
            Status = status;
            TenantId = tenantId;
        }

        public void SetStatus(UserFriendStatus status = UserFriendStatus.NeedValidation)
        {
            if (Status == UserFriendStatus.NeedValidation && status == UserFriendStatus.NeedValidation)
            {
                // 如果是后续验证通过的需要单独的事件
                AddLocalEvent(new UserChatFriendEto
                {
                    TenantId = TenantId,
                    UserId = UserId,
                    FrientId = FrientId,
                    Status = UserFriendStatus.Added
                });
            }
            Status = status;
        }
    }
}
