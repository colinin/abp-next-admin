using LINGYUN.Abp.PushPlus.Channel;
using LINGYUN.Abp.PushPlus.Message;

namespace LINGYUN.Abp.Notifications;
public static class NotificationDefinitionExtensions
{
    private const string Prefix = "push-plus:";
    private const string TemplateKey = Prefix + "template";
    private const string ChannelTypeKey = Prefix + "channel";
    private const string TopicKey = Prefix + "topic";
    /// <summary>
    /// 设定消息模板
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="template"></param>
    /// <returns></returns>
    public static NotificationDefinition WithTemplate(
        this NotificationDefinition notification,
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html)
    {
        return notification.WithProperty(TemplateKey, template);
    }
    /// <summary>
    /// 获取消息模板
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="defaultTemplate"></param>
    /// <returns></returns>
    public static PushPlusMessageTemplate GetTemplateOrDefault(
        this NotificationDefinition notification,
        PushPlusMessageTemplate defaultTemplate = PushPlusMessageTemplate.Html)
    {
        if (notification.Properties.TryGetValue(TemplateKey, out var defineTemplate) == true &&
            defineTemplate is PushPlusMessageTemplate template)
        {
            return template;
        }

        return notification.ContentType switch
        {
            NotificationContentType.Text => PushPlusMessageTemplate.Text,
            NotificationContentType.Html => PushPlusMessageTemplate.Html,
            NotificationContentType.Markdown => PushPlusMessageTemplate.Markdown,
            NotificationContentType.Json => PushPlusMessageTemplate.Json,
            _ => defaultTemplate,
        };
    }
    /// <summary>
    /// 设定消息发送通道
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="channelType"></param>
    /// <returns></returns>
    public static NotificationDefinition WithChannel(
        this NotificationDefinition notification,
        PushPlusChannelType channelType)
    {
        return notification.WithProperty(ChannelTypeKey, channelType);
    }
    /// <summary>
    /// 获取消息发送通道
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="defaultChannelType"></param>
    /// <returns></returns>
    public static PushPlusChannelType GetChannelOrDefault(
        this NotificationDefinition notification,
        PushPlusChannelType defaultChannelType = PushPlusChannelType.WeChat)
    {
        if (notification.Properties.TryGetValue(ChannelTypeKey, out var defineChannelType) == true &&
            defineChannelType is PushPlusChannelType channelType)
        {
            return channelType;
        }

        return defaultChannelType;
    }
    /// <summary>
    /// 消息群发群组编码
    /// see: https://www.pushplus.plus/doc/guide/api.html#%E4%B8%80%E3%80%81%E5%8F%91%E9%80%81%E6%B6%88%E6%81%AF%E6%8E%A5%E5%8F%A3
    /// </summary>
    /// <param name="notification">群组编码</param>
    /// <param name="topic"></param>
    /// <returns>
    /// <see cref="NotificationDefinition"/>
    /// </returns>
    public static NotificationDefinition WithTopic(
        this NotificationDefinition notification,
        string topic)
    {
        return notification.WithProperty(TopicKey, topic);
    }
    /// <summary>
    /// 获取消息群发群组编码
    /// </summary>
    /// <param name="notification"></param>
    /// <returns>
    /// 通知定义的群组编码,未定义返回null
    /// </returns>
    public static string GetTopicOrNull(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(TopicKey, out var topicDefine) == true)
        {
            return topicDefine.ToString();
        }

        return null;
    }
}
