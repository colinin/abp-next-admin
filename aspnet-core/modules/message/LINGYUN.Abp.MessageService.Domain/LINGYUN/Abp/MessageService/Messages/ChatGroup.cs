using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Messages
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
        public virtual string Address { get; protected set; }
        /// <summary>
        /// 群组公告
        /// </summary>
        public virtual string Notice { get; protected set; }
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
        protected ChatGroup() 
        {
        }
        public ChatGroup(long id, string name, string tag = "", string address = "", int maxUserCount = 200)
        {
            GroupId = id;
            Name = name;
            Tag = tag;
            Address = address;
            MaxUserCount = maxUserCount;
        }

        public void ChangeAddress(string address) 
        {
            Address = address;
        }

        public void SetNotice(string notice)
        {
            Notice = notice;
        }
    }
}
