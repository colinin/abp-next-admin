using System;

namespace LINGYUN.Abp.IM.Messages
{
    public class ChatMessage
    {
        public Guid? TenantId { get; set; }
        public string GroupName { get; set; }
        public Guid FormUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
        public MessageType MessageType { get; set; } = MessageType.Text;
    }
}
