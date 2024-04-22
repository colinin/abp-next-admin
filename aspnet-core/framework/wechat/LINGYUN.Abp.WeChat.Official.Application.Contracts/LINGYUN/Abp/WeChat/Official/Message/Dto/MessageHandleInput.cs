using LINGYUN.Abp.WeChat.Official.Models;
using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Official.Message;

[Serializable]
public class MessageHandleInput : WeChatMessage
{
    [DisableAuditing]
    public string Data { get; set; }
}
