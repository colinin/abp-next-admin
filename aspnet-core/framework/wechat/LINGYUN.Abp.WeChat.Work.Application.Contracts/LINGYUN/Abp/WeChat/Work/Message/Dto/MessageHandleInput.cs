using LINGYUN.Abp.WeChat.Work.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Work.Message;

[Serializable]
public class MessageHandleInput : WeChatWorkMessage
{
    [Required]
    [DisableAuditing]
    public string Data { get; set; } = default!;
}
