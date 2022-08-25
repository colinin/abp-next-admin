using LINGYUN.Abp.PushPlus.Channel;
using System;

namespace LINGYUN.Abp.Notifications;
public static class NotificationDefinitionExtensions
{
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
        return notification.WithProperty("channel", channelType);
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
        if (notification.Properties.TryGetValue("channel", out var defineChannelType) == true &&
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
        return notification.WithProperty("topic", topic);
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
        if (notification.Properties.TryGetValue("topic", out var topicDefine) == true)
        {
            return topicDefine.ToString();
        }

        return null;
    }
}
