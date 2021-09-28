using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserChatSetting : Entity<long>, IMultiTenant
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
        /// 允许匿名聊天
        /// </summary>
        public virtual bool AllowAnonymous { get; set; }
        /// <summary>
        /// 允许添加好友
        /// </summary>
        public virtual bool AllowAddFriend { get; set; }
        /// <summary>
        /// 添加好友需要验证
        /// </summary>
        public virtual bool RequireAddFriendValition { get; set; }
        /// <summary>
        /// 允许接收消息
        /// </summary>
        public virtual bool AllowReceiveMessage { get; set; }
        /// <summary>
        /// 允许发送消息
        /// </summary>
        public virtual bool AllowSendMessage { get; set; }
        protected UserChatSetting() 
        {
        }
        public UserChatSetting(Guid userId, Guid? tenantId)
            : this()
        {
            UserId = userId;
            TenantId = tenantId;
            AllowAnonymous = false;
            AllowAddFriend = true;
            AllowReceiveMessage = true;
            AllowSendMessage = true;
            RequireAddFriendValition = true;
        }
    }
}
