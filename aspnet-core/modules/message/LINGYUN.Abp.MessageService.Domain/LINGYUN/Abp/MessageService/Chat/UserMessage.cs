using LINGYUN.Abp.IM.Messages;
using System;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserMessage : Message
    {
        /// <summary>
        /// 接收用户标识
        /// </summary>
        public virtual Guid ReceiveUserId { get; set; }

        protected UserMessage() { }
        public UserMessage(
            long id,
            Guid sendUserId,
            string sendUserName,
            Guid receiveUserId,
            string content,
            MessageType type = MessageType.Text,
            MessageSourceTye source = MessageSourceTye.User,
            Guid? tenantId = null)
            : base(id, sendUserId, sendUserName, content, type, source, tenantId)
        {
            ReceiveUserId = receiveUserId;
        }
    }
}
