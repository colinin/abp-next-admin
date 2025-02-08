using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Messages;
public class EmailMessageAttachment : Entity<long>
{
    public virtual Guid MessageId { get; private set; }
    public virtual string Name { get; private set; }
    public virtual string BlobName { get; private set; }
    public virtual long Size { get; private set; }
    protected EmailMessageAttachment()
    {

    }
    public EmailMessageAttachment(Guid messageId, string name, string blobName, long size)
    {
        MessageId = messageId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EmailMessageAttachmentConsts.MaxNameLength);
        BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName), EmailMessageAttachmentConsts.MaxNameLength);
        Size = size;
    }
}
