using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.IM.Messages
{
    public class ChatMessage
    {
        public Guid? TenantId { get; set; }

        public string GroupId { get; set; }

        public string MessageId { get; set; }

        public Guid FormUserId { get; set; }

        public string FormUserName { get; set; }

        public Guid? ToUserId { get; set; }

        [DisableAuditing]
        public string Content { get; set; }

        public DateTime SendTime { get; set; }

        public bool IsAnonymous { get; set; } = false;

        public MessageType MessageType { get; set; } = MessageType.Text;
    }
}
