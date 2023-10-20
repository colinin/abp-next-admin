using LINGYUN.Abp.IM.Contract;
using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserChatFriendEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 好友标识
        /// </summary>
        public Guid FrientId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public UserFriendStatus Status { get; set; }
    }
}
