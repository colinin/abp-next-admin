using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Account.Models;
public class CreateTicketModel : WeChatRequest
{
    /// <summary>
    /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为60秒。
    /// </summary>
    [JsonProperty("expire_seconds")]
    [JsonPropertyName("expire_seconds")]
    public int? ExpireSeconds { get; set; }
    /// <summary>
    /// 二维码类型
    /// </summary>
    /// <remarks>
    ///     <list type="table">
    ///         <item>QR_SCENE为临时的整型参数值</item>
    ///         <item>QR_STR_SCENE为临时的字符串参数值</item>
    ///         <item>QR_LIMIT_SCENE为永久的整型参数值</item>
    ///         <item>QR_LIMIT_STR_SCENE为永久的字符串参数值</item>
    ///     </list>
    /// </remarks>
    [JsonProperty("action_name")]
    [JsonPropertyName("action_name")]
    public string ActionName { get; private set; }
    /// <summary>
    /// 二维码详细信息
    /// </summary>
    [JsonProperty("action_info")]
    [JsonPropertyName("action_info")]
    public Scene SceneInfo { get; private set; }
    private CreateTicketModel()
    {

    }
    /// <summary>
    /// 通过场景值名称创建临时二维码ticket
    /// </summary>
    /// <param name="sceneStr">场景值名称</param>
    /// <param name="expireSeconds">二维码有效时间</param>
    /// <returns></returns>
    public static CreateTicketModel StringScene(
        string sceneStr,
        int expireSeconds = 60)
    {
        return new CreateTicketModel
        {
            ExpireSeconds = expireSeconds,
            ActionName = "QR_STR_SCENE",
            SceneInfo = new StringScene(sceneStr),
        };
    }
    /// <summary>
    /// 通过场景值名称创建永久二维码ticket
    /// </summary>
    /// <param name="sceneStr">场景值名称</param>
    /// <returns></returns>
    public static CreateTicketModel LimitStringScene(string sceneStr)
    {
        return new CreateTicketModel
        {
            ActionName = "QR_LIMIT_STR_SCENE",
            SceneInfo = new StringScene(sceneStr),
        };
    }
    /// <summary>
    /// 通过场景值标识创建二维码ticket
    /// </summary>
    /// <param name="sceneId">场景值标识</param>
    /// <param name="expireSeconds">二维码有效时间</param>
    /// <returns></returns>
    public static CreateTicketModel EnumScene(
        int sceneId,
        int expireSeconds = 60)
    {
        return new CreateTicketModel
        {
            ExpireSeconds = expireSeconds,
            ActionName = "QR_SCENE",
            SceneInfo = new EnumScene(sceneId),
        };
    }
    /// <summary>
    /// 通过场景值标识创建永久二维码ticket
    /// </summary>
    /// <param name="sceneId">场景值标识</param>
    /// <returns></returns>
    public static CreateTicketModel LimitEnumScene(int sceneId)
    {
        return new CreateTicketModel
        {
            ActionName = "QR_LIMIT_SCENE",
            SceneInfo = new EnumScene(sceneId),
        };
    }
}
