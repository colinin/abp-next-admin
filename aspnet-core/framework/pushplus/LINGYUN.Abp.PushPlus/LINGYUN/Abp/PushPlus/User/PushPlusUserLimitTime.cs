using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.User;

public class PushPlusUserLimitTime
{
    /// <summary>
    /// 发送限制状态;
    /// 1-无限制，
    /// 2-短期限制，
    /// 3-永久限制
    /// </summary>
    [JsonProperty("sendLimit")]
    public PushPlusUserSendLimit SendLimit { get; set; }
    /// <summary>
    /// 解封时间
    /// </summary>
    [JsonProperty("userLimitTime")]
    public string UserLimitTime { get; set; }
}
