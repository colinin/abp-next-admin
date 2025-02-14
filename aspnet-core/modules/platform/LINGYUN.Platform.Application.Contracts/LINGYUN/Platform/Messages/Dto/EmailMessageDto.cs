using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;

namespace LINGYUN.Platform.Messages;

public class EmailMessageDto : MessageDto
{
    public string From { get; set; }
    public string Subject { get; set; }
    public bool IsBodyHtml { get; set; }
    public string CC { get; set; }
    public bool Normalize { get; set; }
    public MailPriority? Priority { get; set; }
    public TransferEncoding? BodyTransferEncoding { get; set; }
    public DeliveryNotificationOptions? DeliveryNotificationOptions { get; set; }
    public ICollection<EmailMessageAttachmentDto> Attachments { get; set; }
    public List<EmailMessageHeaderDto> Headers { get; set; }
}
