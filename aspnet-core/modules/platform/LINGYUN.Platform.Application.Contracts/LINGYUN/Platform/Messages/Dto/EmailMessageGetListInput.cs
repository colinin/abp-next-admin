using System;
using System.Net.Mail;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Messages;
public class EmailMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string EmailAddress { get; set; }
    public string Content { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public MessageStatus? Status { get; set; }
    public MailPriority? Priority { get; set; }
    public DateTime? BeginSendTime { get; set; }
    public DateTime? EndSendTime { get; set; }
}
