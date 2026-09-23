using LINGYUN.Abp.WeChat.Official.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Official.Message;

[Serializable]
public class MessageHandleInput : WeChatMessage
{
    [Required]
    [DisableAuditing]
    public string Data { get; set; } = default!;
}
