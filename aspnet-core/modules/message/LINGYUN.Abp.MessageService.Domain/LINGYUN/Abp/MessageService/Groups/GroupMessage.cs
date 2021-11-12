using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Chat;
using System;

namespace LINGYUN.Abp.MessageService.Groups
{
    public class GroupMessage : Message
    {
        /// <summary>
        /// 群组标识
        /// </summary>
        public virtual long GroupId { get; protected set; }

        protected GroupMessage() { }
        public GroupMessage(
            long id,
            Guid sendUserId,
            string sendUserName,
            long groupId,
            string content,
            MessageType type = MessageType.Text,
            MessageSourceTye source = MessageSourceTye.User,
            Guid? tenantId = null)
            : base(id, sendUserId, sendUserName, content, type, source, tenantId)
        {
            GroupId = groupId;
        }
    }
}
