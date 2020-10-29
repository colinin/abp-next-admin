using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Chat;
using System;

namespace LINGYUN.Abp.MessageService.Group
{
    public class GroupMessage : Message
    {
        /// <summary>
        /// 群组标识
        /// </summary>
        public virtual long GroupId { get; protected set; }

        protected GroupMessage() { }
        public GroupMessage(long id, Guid sendUserId, string sendUserName, string content, MessageType type = MessageType.Text)
            : base(id, sendUserId, sendUserName, content, type)
        {

        }
        public void SendToGroup(long groupId)
        {
            GroupId = groupId;
        }
    }
}
