using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Timing;

namespace LINGYUN.Platform.Messages;

public class Message : AuditedAggregateRoot<Guid>
{
    public virtual Guid? UserId { get; private set; }
    public virtual string Sender { get; private set; }
    public virtual string Provider { get; set; }
    public virtual string Receiver { get; private set; }
    public virtual string Content { get; private set; }
    public virtual DateTime? SendTime { get; private set; }
    public virtual int SendCount { get; private set; }
    public virtual MessageStatus Status { get; private set; }
    public virtual string Reason { get; private set; }

    protected Message()
    {

    }

    public Message(
        Guid id,
        string receiver,
        string content,
        Guid? userId = null, 
        string userName = null)
        : base(id)
    {
        Receiver = Check.NotNullOrWhiteSpace(receiver, nameof(receiver), MessageConsts.MaxReceiverLength);
        Content = content;

        UserId = userId;
        Sender = Check.Length(userName, nameof(userName), MessageConsts.MaxSenderLength);
        SendCount = 0;
        Status = MessageStatus.Pending;
    }

    public void Sent(IClock clock)
    {
        SendCount += 1;
        SendTime = clock.Now;
        Status = MessageStatus.Sent;
        Reason = "";
    }

    public void Failed(string error, IClock clock)
    {
        SendTime = clock.Now;
        Status = MessageStatus.Failed;
        Reason = error.Truncate(MessageConsts.MaxReasonLength);
    }
}
