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
        public UserMessage(long id, Guid sendUserId, string sendUserName, string content, MessageType type = MessageType.Text)
            : base(id, sendUserId, sendUserName, content, type)
        {

        }

        public void SendToUser(Guid receiveUserId)
        {
            ReceiveUserId = receiveUserId;
        }
    }
}
