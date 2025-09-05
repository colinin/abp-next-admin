using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.JsSdk.Models;

public class JsApiTicketInfoResponse : WeChatWorkResponse
{
    /// <summary>
    /// 生成签名所需的 jsapi_ticket，最长为512字节
    /// </summary>
    [JsonProperty("ticket")]
    [JsonPropertyName("ticket")]
    public string Ticket { get; set; }
    /// <summary>
    /// 凭证的有效时间（秒）
    /// </summary>
    [JsonProperty("expires_in")]
    [JsonPropertyName("expires_in")]
    [System.Text.Json.Serialization.JsonConverter(typeof(NumberToStringConverter))]
    public int ExpiresIn { get; set; }

    public JsApiTicketInfo ToJsApiTicket()
    {
        ThrowIfNotSuccess();
        return new JsApiTicketInfo(Ticket, ExpiresIn);
    }
}
