using System;

namespace LINGYUN.Platform.Messages;
public abstract class MessageEto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Sender { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? CreatorId { get; set; }
    public MessageStatus Status { get; set; }
}
