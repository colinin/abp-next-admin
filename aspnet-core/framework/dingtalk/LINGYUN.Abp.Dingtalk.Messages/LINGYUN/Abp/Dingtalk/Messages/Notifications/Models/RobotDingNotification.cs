using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Dingtalk.Messages.Notifications.Models;
/// <summary>
/// 机器人DING消息
/// </summary>
public class RobotDingNotification : RobotNotification
{
    /// <summary>
    /// 机器人ID，需填写创建企业内部应用机器人后获取的机器人 ID（robotCode）
    /// </summary>
    [JsonPropertyName("robotCode")]
    public virtual string RobotCode { get; }
    /// <summary>
    /// 接收人userId列表，可通过查询用户详情或获取部门用户userid列表接口获取。
    /// </summary>
    [JsonPropertyName("receiverUserIdList")]
    public virtual string[] ReceiverUserIdList { get; }
    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonPropertyName("content")]
    public virtual string Content { get; }
    /// <summary>
    /// DING消息类型
    /// </summary>
    [JsonPropertyName("remindType")]
    public RemindType RemindType { get; }
    /// <summary>
    /// 电话音色，非电话DING该字段无效
    /// </summary>
    [JsonPropertyName("callVoice")]
    public string? CallVoice { get; }
    public RobotDingNotification(
        string robotCode, 
        string content, 
        string[] receiverUserIdList,
        RemindType remindType,
        CallVoice? callVoice = null)
    {
        RobotCode = robotCode;
        ReceiverUserIdList = receiverUserIdList;
        Content = content;
        RemindType = remindType;
        CallVoice = callVoice?.ToString();
    }
}
