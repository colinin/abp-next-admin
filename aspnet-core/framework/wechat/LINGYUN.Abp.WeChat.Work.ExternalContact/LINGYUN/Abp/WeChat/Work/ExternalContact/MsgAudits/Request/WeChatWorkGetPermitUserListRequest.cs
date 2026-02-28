using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Request;
/// <summary>
/// 获取会话内容存档开启成员列表请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91614" />
/// </remarks>
public class WeChatWorkGetPermitUserListRequest : WeChatWorkRequest
{
    /// <summary>
    /// 拉取对应版本的开启成员列表。1表示办公版；2表示服务版；3表示企业版。非必填，不填写的时候返回全量成员列表
    /// </summary>
    [CanBeNull]
    [JsonProperty("cursor")]
    [JsonPropertyName("cursor")]
    public int? Type { get; }
    private WeChatWorkGetPermitUserListRequest(int? type = null)
    {
        Type = type; 
    }
    /// <summary>
    /// 默认
    /// </summary>
    /// <returns></returns>
    public static WeChatWorkGetPermitUserListRequest Default()
    {
        return new WeChatWorkGetPermitUserListRequest();
    }
    /// <summary>
    /// 办公版
    /// </summary>
    /// <returns></returns>
    public static WeChatWorkGetPermitUserListRequest Office()
    {
        return new WeChatWorkGetPermitUserListRequest(1);
    }
    /// <summary>
    /// 服务版
    /// </summary>
    /// <returns></returns>
    public static WeChatWorkGetPermitUserListRequest Service()
    {
        return new WeChatWorkGetPermitUserListRequest(2);
    }
    /// <summary>
    /// 企业版
    /// </summary>
    /// <returns></returns>
    public static WeChatWorkGetPermitUserListRequest Enterprise()
    {
        return new WeChatWorkGetPermitUserListRequest(3);
    }
}
