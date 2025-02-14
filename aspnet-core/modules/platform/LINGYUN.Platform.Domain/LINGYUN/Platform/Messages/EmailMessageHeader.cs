using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Messages;
public class EmailMessageHeader : Entity<long>
{
    public virtual Guid MessageId { get; private set; }
    public virtual string Key { get; private set; }
    public virtual string Value { get; private set; }
    protected EmailMessageHeader()
    {
    }
    public EmailMessageHeader(Guid messageId, string key, string value)
    {
        MessageId = messageId;
        Key = Check.NotNullOrWhiteSpace(key, nameof(key), EmailMessageHeaderConsts.MaxKeyLength);
        Value = Check.NotNullOrWhiteSpace(value, nameof(value), EmailMessageHeaderConsts.MaxValueLength);
    }
}
