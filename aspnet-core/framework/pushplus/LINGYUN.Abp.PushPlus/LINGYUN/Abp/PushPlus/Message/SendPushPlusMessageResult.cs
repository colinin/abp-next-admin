using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.PushPlus.Message;

public class SendPushPlusMessageResult
{
    /// <summary>
    /// 消息投递状态
    /// 0-未投递，
    /// 1-发送中，
    /// 2-已发送，
    /// 3-发送失败
    /// </summary>
    [JsonProperty("status")] 
    public PushPlusMessageStatus Status { get; set; }
    /// <summary>
    /// 发送失败原因
    /// </summary>
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}
