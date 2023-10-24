using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.Setting;
/// <summary>
/// 发送渠道
/// </summary>
public class PushPlusChannel
{
    /// <summary>
    /// 默认渠道编码
    /// </summary>
    [JsonProperty("defaultChannel")]
    public string DefaultChannel { get; set; }
    /// <summary>
    /// 默认渠道名称
    /// </summary>
    [JsonProperty("defaultChannelTxt")]
    public string DefaultChannelName { get; set; }
    /// <summary>
    /// 渠道参数
    /// </summary>
    [JsonProperty("defaultWebhook")]
    public string DefaultWebhook { get; set; }
    /// <summary>
    /// 发送限制；0-无限制，1-禁止所有渠道发送，2-限制微信渠道，3-限制邮件渠道
    /// </summary>
    [JsonProperty("sendLimit")]
    public PushPlusChannelRecevieLimit SendLimit { get; set; }
    /// <summary>
    /// 接收限制；0-接收全部，1-不接收消息
    /// </summary>
    [JsonProperty("recevieLimit")]
    public PushPlusChannelRecevieLimit RecevieLimit { get; set; }

}
