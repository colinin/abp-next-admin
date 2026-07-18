using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Dingtalk.Messages.Notifications.Models;

public abstract class RobotWebhookNotification : RobotNotification
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonPropertyName("msgtype")]
    public string MsgType { get; }
    /// <summary>
    /// 消息幂等key，可用于控制消息幂等
    /// </summary>
    [JsonPropertyName("msgUuid")]
    public string? MsgUuid { get; }
    protected RobotWebhookNotification(string msgType, string? msgUuid = null)
    {
        MsgType = msgType;
        MsgUuid = msgUuid;
    }
}

/// <summary>
/// 文本类型消息
/// </summary>
public class RobotWebhookTextNotification : RobotWebhookNotification
{
    /// <summary>
    /// 文本消息的内容
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; }
    public RobotWebhookTextNotification(
        string content, 
        string? msgUuid = null) : base("text", msgUuid)
    {
        Content = content;
    }
}
/// <summary>
/// 被@的群成员信息
/// </summary>
public class RobotWebhookAtNotification : RobotWebhookNotification
{
    /// <summary>
    /// 是否@所有人
    /// </summary>
    [JsonPropertyName("isAtAll")]
    public bool IsAtAll { get; }
    /// <summary>
    /// 被@的群成员手机号
    /// </summary>
    [JsonPropertyName("atMobiles")]
    public string[]? AtMobiles { get; }
    /// <summary>
    /// 被@的群成员userId
    /// </summary>
    /// <remarks>
    /// 在@群成员时，最多只能@50个
    /// </remarks>
    [JsonPropertyName("atUserIds")]
    public string[]? AtUserIds { get; }
    public RobotWebhookAtNotification(
        bool isAtAll = false,
        string[]? atMobiles = null,
        string[]? atUserIds = null,
        string? msgUuid = null) : base("at", msgUuid)
    {
        IsAtAll = isAtAll;
        AtMobiles = atMobiles;
        AtUserIds = atUserIds;
    }
}
/// <summary>
/// 链接类型消息
/// </summary>
public class RobotWebhookLinkNotification : RobotWebhookNotification
{
    /// <summary>
    /// 链接消息标题
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    /// <summary>
    /// 链接消息的内容
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; }
    /// <summary>
    /// 链接消息内的图片地址，建议使用上传媒体文件接口获取
    /// </summary>
    [JsonPropertyName("picUrl")]
    public string? PicUrl { get; }
    /// <summary>
    /// 点击消息跳转的URL
    /// </summary>
    [JsonPropertyName("messageUrl")]
    public string? MessageUrl { get; }
    public RobotWebhookLinkNotification(
        string title,
        string text,
        string? picUrl = null,
        string? messageUrl = null,
        string? msgUuid = null) : base("link", msgUuid)
    {
        Title = title;
        Text = text;
        PicUrl = picUrl;
        MessageUrl = messageUrl;
    }
}
/// <summary>
/// markdown类型消息
/// </summary>
public class RobotWebhookMarkdownNotification : RobotWebhookNotification
{
    /// <summary>
    /// 消息会话列表中展示的标题，非消息体的标题
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    /// <summary>
    /// markdown类型消息的文本内容
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; }
    public RobotWebhookMarkdownNotification(
        string title,
        string text,
        string? msgUuid = null) : base("markdown", msgUuid)
    {
        Title = title;
        Text = text;
    }
}
/// <summary>
/// actionCard类型消息
/// </summary>
public class RobotWebhookActionCardNotification : RobotWebhookNotification
{
    /// <summary>
    /// 是否显示消息发送者头像
    /// </summary>
    [JsonPropertyName("hideAvatar")]
    public string HideAvatar { get; }
    /// <summary>
    /// 消息内按钮排列方式
    /// </summary>
    [JsonPropertyName("btnOrientation")]
    public string BtnOrientation { get; }
    /// <summary>
    /// 点击singleTitle按钮触发的URL
    /// </summary>
    /// <remarks>
    /// 消息内只有一个按钮时，该参数必填
    /// </remarks>
    [JsonPropertyName("singleURL")]
    public string? SingleURL { get; }
    /// <summary>
    /// 单个按钮的方案。(设置此项和singleURL后btns无效。)
    /// </summary>
    /// <remarks>
    /// 消息内只有一个按钮时，该参数必填
    /// </remarks>
    [JsonPropertyName("singleTitle")]
    public string? SingleTitle { get; }
    /// <summary>
    /// 消息会话列表中展示的标题，非消息体的标题
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    /// <summary>
    /// actionCard类型消息的正文内容，支持markdown语法
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; }
    /// <summary>
    /// 按钮的信息列表
    /// </summary>
    /// <remarks>
    /// 消息内不止一个按钮时，该参数必填
    /// </remarks>
    [JsonPropertyName("btns")]
    public RobotWebhookActionCardButton[]? Buttons { get; }
    public RobotWebhookActionCardNotification(
        string title,
        string text,
        string? singleTitle = null,
        string? singleURL = null,
        bool hideAvatar = true,
        bool isButtonHorizontally = true,
        RobotWebhookActionCardButton[]? buttons = null,
        string? msgUuid = null) : base("actionCard", msgUuid)
    {
        Title = title;
        Text = text;
        SingleTitle = singleTitle;
        SingleURL = singleURL;
        HideAvatar = hideAvatar ? "1" : "0";
        BtnOrientation = isButtonHorizontally ? "1" : "0";
        Buttons = buttons;
    }
}
/// <summary>
/// feedCard类型消息
/// </summary>
public class RobotWebhookFeedCardNotification : RobotWebhookNotification
{
    /// <summary>
    /// feedCard消息的内容列表
    /// </summary>
    [JsonPropertyName("links")]
    public RobotWebhookFeedCardLink[] Links { get; }
    public RobotWebhookFeedCardNotification(
        RobotWebhookFeedCardLink[] links,
        string? msgUuid = null) : base("feedCard", msgUuid)
    {
        Links = links;
    }
}
public class RobotWebhookFeedCardLink
{
    /// <summary>
    /// feedCard消息内每条内容的图片URL，建议使用上传媒体文件接口获取
    /// </summary>
    [JsonPropertyName("picURL")]
    public string? PicURL { get; }
    /// <summary>
    /// feedCard消息内每条内容上跳转链接
    /// </summary>
    [JsonPropertyName("messageURL")]
    public string MessageURL { get; }
    /// <summary>
    /// feedCard消息内每条内容的标题
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    public RobotWebhookFeedCardLink(
        string title,
        string messageURL,
        string? picURL = null)
    {
        Title = title;
        MessageURL = messageURL;
        PicURL = picURL;
    }
}
public class RobotWebhookActionCardButton
{
    /// <summary>
    /// 按钮跳转的URL
    /// </summary>
    [JsonPropertyName("actionURL")]
    public string ActionURL { get; }
    /// <summary>
    /// 按钮上显示的文本
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }
    public RobotWebhookActionCardButton(string title, string actionURL)
    {
        Title = title;
        ActionURL = actionURL;
    }
}