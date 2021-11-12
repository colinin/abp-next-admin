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
        /// 系统预置
        /// </summary>
        public virtual bool IsStatic { get; set; }
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
        /// <summary>
        /// 附加说明
        /// </summary>
        public virtual string Description { get; set; }

        public virtual UserFriendStatus Status { get; protected set; }

        protected UserChatFriend()
        {
        }

        public UserChatFriend(
            Guid userId,
            Guid friendId,
            string remarkName = "",
            string description = "",
            Guid? tenantId = null)
        {
            UserId = userId;
            FrientId = friendId;
            RemarkName = remarkName;
            TenantId = tenantId;
            Description = description;
            Status = UserFriendStatus.NeedValidation;
        }

        public void SetStatus(UserFriendStatus status = UserFriendStatus.NeedValidation)
        {
            if (Status == UserFriendStatus.NeedValidation && status == UserFriendStatus.Added)
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
