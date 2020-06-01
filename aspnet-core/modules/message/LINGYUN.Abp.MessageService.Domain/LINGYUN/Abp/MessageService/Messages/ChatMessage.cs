using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Messages
{
    public abstract class ChatMessage : Entity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 发送用户标识
        /// </summary>
        public virtual Guid SendUserId { get; protected set; }
        /// <summary>
        /// 接收用户标识
        /// </summary>
        public virtual Guid ReceiveUserId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; protected set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public virtual SendStatus SendStatus { get; protected set; }
    }
}
