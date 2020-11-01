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
        /// 发送状态
        /// </summary>
        public virtual MessageSendState SendState { get; protected set; }
        protected Message() { }
        public Message(long id, Guid sendUserId, string sendUserName, string content, MessageType type = MessageType.Text)
        {
            MessageId = id;
            CreatorId = sendUserId;
            SendUserName = sendUserName;
            Content = content;
            Type = type;
            CreationTime = DateTime.Now;
        }

        public void ChangeSendState(MessageSendState state = MessageSendState.Send)
        {
            SendState = state;
        }
    }
}
