using LINGYUN.Abp.WeChat.Work.Models;
using System;

namespace LINGYUN.Abp.WeChat.Work.Message;

[Serializable]
public class MessageHandleInput : WeChatWorkMessage
{
    public string Data { get; set; }
}
