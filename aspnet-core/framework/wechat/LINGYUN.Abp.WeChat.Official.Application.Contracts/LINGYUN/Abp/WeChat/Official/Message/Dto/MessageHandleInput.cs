using LINGYUN.Abp.WeChat.Official.Models;
using System;

namespace LINGYUN.Abp.WeChat.Official.Message;

[Serializable]
public class MessageHandleInput : WeChatMessage
{
    public string Data { get; set; }
}
