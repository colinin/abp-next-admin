using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Messages;
public abstract class MessageDto : AuditedEntityDto<Guid>
{
    public Guid? UserId { get; set; }
    public string Sender { get; set; }
    public string Provider { get; set; }
    public string Receiver { get; set; }
    public string Content { get; set; }
    public DateTime? SendTime { get; set; }
    public int SendCount { get; set; }
    public MessageStatus Status { get; set; }
    public string Reason { get; set; }
}
