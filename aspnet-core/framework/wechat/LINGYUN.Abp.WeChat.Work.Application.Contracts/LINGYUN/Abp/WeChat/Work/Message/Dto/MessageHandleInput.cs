using LINGYUN.Abp.WeChat.Work.Models;
using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Work.Message;

[Serializable]
public class MessageHandleInput : WeChatWorkMessage
{
    [DisableAuditing]
    public string Data { get; set; }
}
