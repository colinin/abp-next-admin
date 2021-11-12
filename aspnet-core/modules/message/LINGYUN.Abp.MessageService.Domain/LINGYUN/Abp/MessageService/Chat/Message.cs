using LINGYUN.Abp.IM.Messages;
using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    public abstract class Message : CreationAuditedAggregateRoot<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 消息标识
        /// </summary>
        public virtual long MessageId { get; protected set; }
        /// <summary>
        /// 发送用户名称
        /// </summary>
        public virtual string SendUserName { get; protected set; }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; protected set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual MessageType Type { get; protected set; }
        /// <summary>
        /// 消息来源
        /// </summary>
        public virtual MessageSourceTye Source { get; protected set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public virtual MessageState State { get; protected set; }
        protected Message() { }
        protected Message(
            long id,
            Guid sendUserId,
            string sendUserName,
            string content,
            MessageType type = MessageType.Text,
            MessageSourceTye source = MessageSourceTye.User,
            Guid? tenantId = null)
        {
            MessageId = id;
            CreatorId = sendUserId;
            SendUserName = sendUserName;
            Content = content;
            Type = type;
            Source = source;
            CreationTime = DateTime.Now;
            TenantId = tenantId;
            ChangeSendState();
        }

        public void ChangeSendState(MessageState state = MessageState.Send)
        {
            State = state;
        }
    }
}
