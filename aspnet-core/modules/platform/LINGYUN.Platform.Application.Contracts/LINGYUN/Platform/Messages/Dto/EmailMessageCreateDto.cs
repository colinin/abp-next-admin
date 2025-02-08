using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net.Mime;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Messages;
public class EmailMessageCreateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(MessageConsts), nameof(MessageConsts.MaxReceiverLength))]
    public string To { get; set; }

    [Required]
    public string Content { get; set; }

    [DynamicStringLength(typeof(EmailMessageConsts), nameof(EmailMessageConsts.MaxFromLength))]
    public string From { get; set; }

    [DynamicStringLength(typeof(EmailMessageConsts), nameof(EmailMessageConsts.MaxSubjectLength))]
    public string Subject { get; set; }

    public bool IsBodyHtml { get; set; } = true;

    [DynamicStringLength(typeof(MessageConsts), nameof(MessageConsts.MaxReceiverLength))]
    public string CC { get; set; }

    public bool Normalize { get; set; }

    public MailPriority? Priority { get; set; }
    public TransferEncoding? BodyTransferEncoding { get; set; }
    public DeliveryNotificationOptions? DeliveryNotificationOptions { get; set; }

    [DisableAuditing]
    public IRemoteStreamContent[] Attachments { get; set; }

    public List<EmailMessageHeaderDto> Headers { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public EmailMessageCreateDto()
    {

    }

    public EmailMessageCreateDto(
        [NotNull] string to,
        [NotNull] string content, 
        string from = null,
        string subject = null, 
        bool isBodyHtml = true, 
        string cc = null)
    {
        To = to;
        Content = content;
        From = from;
        Subject = subject;
        IsBodyHtml = isBodyHtml;
        CC = cc;
    }
}
