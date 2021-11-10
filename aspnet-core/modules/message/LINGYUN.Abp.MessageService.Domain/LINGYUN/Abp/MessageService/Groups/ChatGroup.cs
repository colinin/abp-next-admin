using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Groups
{
    /// <summary>
    /// 聊天群组
    /// </summary>
    public class ChatGroup : AuditedEntity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 群主
        /// </summary>
        public virtual Guid AdminUserId { get; protected set; }
        /// <summary>
        /// 群组标识
        /// </summary>
        public virtual long GroupId { get; protected set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// 群组标记
        /// </summary>
        public virtual string Tag { get; protected set; }
        /// <summary>
        /// 群组地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 群组公告
        /// </summary>
        public virtual string Notice { get; set; }
        /// <summary>
        /// 最大用户数量
        /// </summary>
        public virtual int MaxUserCount { get; protected set; }
        /// <summary>
        /// 允许匿名聊天
        /// </summary>
        public virtual bool AllowAnonymous { get; set; }
        /// <summary>
        /// 允许发送消息
        /// </summary>
        public virtual bool AllowSendMessage { get; set; }
        /// <summary>
        /// 群组说明
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 群组头像地址
        /// </summary>
        public virtual string AvatarUrl { get; set; }
        protected ChatGroup()
        {
        }
        public ChatGroup(
            long id,
            Guid adminUserId,
            string name,
            string tag = "",
            string address = "",
            int maxUserCount = 200)
        {
            GroupId = id;
            AdminUserId = adminUserId;
            Name = name;
            Tag = tag;
            Address = address;
            MaxUserCount = maxUserCount;
        }
    }
}
