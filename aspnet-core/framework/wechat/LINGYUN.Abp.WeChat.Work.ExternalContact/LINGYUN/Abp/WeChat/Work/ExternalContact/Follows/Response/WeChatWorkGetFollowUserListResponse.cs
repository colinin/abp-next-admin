using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Follows.Response;
/// <summary>
/// 获取配置了客户联系功能的成员列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91853" />
/// </remarks>
public class WeChatWorkGetFollowUserListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 配置了客户联系功能的成员userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("follow_user")]
    [JsonPropertyName("follow_user")]
    public List<string> FollowUser {  get; set; } = new List<string>();
}
