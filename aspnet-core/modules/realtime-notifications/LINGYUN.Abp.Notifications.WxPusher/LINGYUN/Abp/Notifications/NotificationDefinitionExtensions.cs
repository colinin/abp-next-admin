using LINGYUN.Abp.WxPusher.Messages;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications;

public static class NotificationDefinitionExtensions
{
    private const string Prefix = "wx-pusher:";
    private const string ContentTypeKey = Prefix + "contentType";
    private const string TopicKey = Prefix + "topic";
    private const string UrlKey = Prefix + "url";
    /// <summary>
    /// 设定消息内容类型
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public static NotificationDefinition WithContentType(
        this NotificationDefinition notification,
        MessageContentType contentType)
    {
        return notification.WithProperty(ContentTypeKey, contentType);
    }
    /// <summary>
    /// 获取消息发送通道
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="defaultChannelType"></param>
    /// <returns></returns>
    public static MessageContentType GetContentTypeOrDefault(
        this NotificationDefinition notification,
        MessageContentType defaultContentType = MessageContentType.Text)
    {
        if (notification.Properties.TryGetValue(ContentTypeKey, out var defineContentType) == true &&
            defineContentType is MessageContentType contentType)
        {
            return contentType;
        }

        return notification.ContentType switch
        {
            NotificationContentType.Text => MessageContentType.Text,
            NotificationContentType.Html => MessageContentType.Html,
            NotificationContentType.Markdown => MessageContentType.Markdown,
            NotificationContentType.Json => MessageContentType.Text,
            _ => defaultContentType,
        };
    }
    /// <summary>
    /// 消息主题(Topic)
    /// see: https://wxpusher.dingliqc.com/docs/#/?id=%e5%90%8d%e8%af%8d%e8%a7%a3%e9%87%8a
    /// </summary>
    /// <param name="notification">群组编码</param>
    /// <param name="topics"></param>
    /// <returns>
    /// <see cref="NotificationDefinition"/>
    /// </returns>
    public static NotificationDefinition WithTopics(
        this NotificationDefinition notification,
        List<int> topics)
    {
        return notification.WithProperty(TopicKey, topics);
    }
    /// <summary>
    /// 获取消息群发群组编码
    /// </summary>
    /// <param name="notification"></param>
    /// <returns>
    /// 通知定义的群组编码列表
    /// </returns>
    public static List<int> GetTopics(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(TopicKey, out var topicsDefine) == true &&
            topicsDefine is List<int> topics)
        {
            return topics;
        }

        return new List<int>();
    }
    /// <summary>
    /// 用户点击标题跳转页面
    /// </summary>
    /// <param name="notification">群组编码</param>
    /// <param name="url"></param>
    /// <returns>
    /// <see cref="NotificationDefinition"/>
    /// </returns>
    public static NotificationDefinition WithUrl(
        this NotificationDefinition notification,
        string url)
    {
        return notification.WithProperty(UrlKey, url);
    }
    /// <summary>
    /// 获取标题跳转页面
    /// </summary>
    /// <param name="notification"></param>
    public static string GetUrlOrNull(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(UrlKey, out var urlDefine))
        {
            return urlDefine.ToString();
        }

        return null;
    }
}
