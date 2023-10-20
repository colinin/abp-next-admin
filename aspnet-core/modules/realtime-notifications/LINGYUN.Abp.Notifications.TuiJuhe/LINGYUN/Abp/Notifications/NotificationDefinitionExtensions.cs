using LINGYUN.Abp.TuiJuhe.Messages;

namespace LINGYUN.Abp.Notifications;

public static class NotificationDefinitionExtensions
{
    private const string Prefix = "tui-juhe:";
    private const string ServiceIdKey = Prefix + "serviceId";

    /// <summary>
    /// 获取消息内容类型
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="defaultContentType"></param>
    /// <returns></returns>
    public static MessageContentType GetContentTypeOrDefault(
        this NotificationDefinition notification,
        MessageContentType defaultContentType = MessageContentType.Text)
    {
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
    /// 指定服务编号
    /// </summary>
    /// <param name="notification">群组编码</param>
    /// <param name="serviceId">服务编号，在服务创建后自动生成ServiceID，是服务的唯一标识，可在每个服务的详情页中查看</param>
    /// <returns>
    /// <see cref="NotificationDefinition"/>
    /// </returns>
    public static NotificationDefinition WithServiceId(
        this NotificationDefinition notification,
        string serviceId)
    {
        return notification.WithProperty(ServiceIdKey, serviceId);
    }
    /// <summary>
    /// 获取服务编号
    /// </summary>
    /// <param name="notification"></param>
    public static string GetServiceIdOrNull(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(ServiceIdKey, out var serviceIdDefine))
        {
            return serviceIdDefine.ToString();
        }

        return null;
    }
}
