using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
/// <summary>
/// 获取加入企业二维码响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91714" />
/// </remarks>
public class WeChatWorkGetJoinQrCodeResponse : WeChatWorkResponse
{
    /// <summary>
    /// 二维码链接，有效期7天
    /// </summary>
    [NotNull]
    [JsonProperty("join_qrcode")]
    [JsonPropertyName("join_qrcode")]
    public string JoinQrcCode { get; set; }
}
