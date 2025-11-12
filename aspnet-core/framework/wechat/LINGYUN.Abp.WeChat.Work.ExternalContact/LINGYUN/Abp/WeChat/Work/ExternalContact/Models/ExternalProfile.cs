using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Models;
/// <summary>
/// 成员对外信息
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(ExternalProfileSystemTextJsonConverter))]
[Newtonsoft.Json.JsonConverter(typeof(ExternalProfileNewtonsoftJsonConverter))]
public class ExternalProfile
{
    /// <summary>
    /// 企业对外简称，需从已认证的企业简称中选填。可在“我的企业”页中查看企业简称认证状态。
    /// </summary>
    [NotNull]
    [JsonProperty("external_corp_name")]
    [JsonPropertyName("external_corp_name")]
    public string ExternalCorpName {  get; set; }
    /// <summary>
    /// 视频号属性。须从企业绑定到企业微信的视频号中选择，可在“我的企业”页中查看绑定的视频号。
    /// 第三方仅通讯录应用可获取；对于非第三方创建的成员，第三方通讯录应用也不可获取
    /// </summary>
    [NotNull]
    [JsonProperty("wechat_channels")]
    [JsonPropertyName("wechat_channels")]
    public List<WechatChannel> WechatChannels { get; set; }
    /// <summary>
    /// 属性列表
    /// </summary>
    [NotNull]
    [JsonProperty("external_attr")]
    [JsonPropertyName("external_attr")]
    public List<ExternalAttribute> ExternalAttributes { get; set; }
}
