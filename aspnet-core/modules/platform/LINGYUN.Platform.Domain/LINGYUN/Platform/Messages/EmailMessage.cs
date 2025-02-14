using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using Volo.Abp;

namespace LINGYUN.Platform.Messages;
public class EmailMessage : Message
{
    public virtual string From { get; set; }
    public virtual string Subject { get; private set; }
    public virtual bool IsBodyHtml { get; private set; }
    public virtual string CC { get; private set; }
    public virtual bool Normalize { get; set; }
    public virtual MailPriority? Priority { get; set; }
    public virtual TransferEncoding? BodyTransferEncoding { get; set; }
    public virtual DeliveryNotificationOptions? DeliveryNotificationOptions { get; set; }
    public virtual ICollection<EmailMessageAttachment> Attachments { get; private set; }
    public virtual ICollection<EmailMessageHeader> Headers { get; private set; }
    protected EmailMessage()
    {
        Headers = new Collection<EmailMessageHeader>();
        Attachments = new Collection<EmailMessageAttachment>();
    }
    public EmailMessage(
        Guid id,
        string to,
        string from,
        string subject,
        string body,
        bool isBodyHtml = false,
        string cc = null,
        Guid? userId = null,
        string userName = null)
        : base(id, to, body, userId, userName)
    {
        From = Check.Length(from, nameof(from), EmailMessageConsts.MaxFromLength);
        Subject = Check.Length(subject, nameof(subject), EmailMessageConsts.MaxSubjectLength);
        IsBodyHtml = isBodyHtml;
        CC = Check.Length(cc, nameof(cc), MessageConsts.MaxReceiverLength);

        Headers = new Collection<EmailMessageHeader>();
        Attachments = new Collection<EmailMessageAttachment>();
    }

    public bool IsInAttachment(string name)
    {
        return Attachments.Any(x => x.Name == name);
    }

    public void RemoveAttachment(string name)
    {
        Attachments.RemoveAll(x => x.Name == name);
    }

    public void AddAttachment(string name, string blobName, long size)
    {
        if (IsInAttachment(name))
        {
            throw new BusinessException(PlatformErrorCodes.DuplicateEmailMessageAttachment)
                .WithData("Name", name);
        }
        Attachments.Add(new EmailMessageAttachment(Id, name, blobName, size));
    }

    public bool IsInHeader(string key)
    {
        return Headers.Any(x => x.Key == key);
    }

    public void RemoveHeader(string key)
    {
        Headers.RemoveAll(x => x.Key == key);
    }

    public void AddHeader(string key, string value)
    {
        if (IsInHeader(key))
        {
            throw new BusinessException(PlatformErrorCodes.DuplicateEmailMessageHeader)
                .WithData("Key", key);
        }
        Headers.Add(new EmailMessageHeader(Id, key, value));
    }
}
