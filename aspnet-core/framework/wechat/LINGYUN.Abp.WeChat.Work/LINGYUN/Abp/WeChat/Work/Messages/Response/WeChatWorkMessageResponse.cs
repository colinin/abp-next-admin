using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Response;
/// <summary>
/// 企业微信发送消息响应
/// </summary>
public class WeChatWorkMessageResponse : WeChatWorkResponse
{
    /// <summary>
    /// 不合法的userid，不区分大小写，统一转为小写
    /// </summary>
    [JsonProperty("invaliduser")]
    [JsonPropertyName("invaliduser")]
    public string InvalidUser { get; set; }
    /// <summary>
    /// 不合法的partyid
    /// </summary>
    [JsonProperty("invalidparty")]
    [JsonPropertyName("invalidparty")]
    public string InvalidParty { get; set; }
    /// <summary>
    /// 不合法的标签id
    /// </summary>
    [JsonProperty("invalidtag")]
    [JsonPropertyName("invalidtag")]
    public string InvalidTag { get; set; }
    /// <summary>
    /// 没有基础接口许可(包含已过期)的userid
    /// </summary>
    [JsonProperty("unlicenseduser")]
    [JsonPropertyName("unlicenseduser")]
    public string UnLicensedUser { get; set; }
    /// <summary>
    /// 消息id，用于撤回应用消息
    /// </summary>
    [JsonProperty("msgid")]
    [JsonPropertyName("msgid")]
    public string MsgId { get; set; }
    /// <summary>
    /// 仅消息类型为“按钮交互型”，“投票选择型”和“多项选择型”的模板卡片消息返回，
    /// 应用可使用response_code调用更新模版卡片消息接口，72小时内有效，且只能使用一次
    /// </summary>
    [JsonProperty("response_code")]
    [JsonPropertyName("response_code")]
    public string ResponseCode { get; set; }
}
