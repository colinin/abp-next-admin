using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Messages;
public class SmsMessageGetListInput : PagedAndSortedResultRequestDto
{
    public string PhoneNumber { get; set; }
    public string Content { get; set; }
    public MessageStatus? Status { get; set; }
    public DateTime? BeginSendTime { get; set; }
    public DateTime? EndSendTime { get; set; }
}
