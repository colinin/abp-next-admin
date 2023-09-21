namespace LINGYUN.Abp.Notifications;
public static class NotificationDefinitionWeChatWorkExtensions
{
    private const string Prefix = "wx-work:";
    private const string AgentIdKey = Prefix + "agent_id";
    private const string ToTagKey = Prefix + "to_tag";
    private const string ToPartyKey = Prefix + "to_party";

    /// <summary>
    /// 设定发送到所有应用
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public static NotificationDefinition WithAllAgent(
        this NotificationDefinition notification)
    {
        return notification.WithAgentId("@all");
    }

    /// <summary>
    /// 设定消息应用标识
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="agentId"></param>
    /// <returns></returns>
    public static NotificationDefinition WithAgentId(
        this NotificationDefinition notification,
        string agentId)
    {
        return notification.WithProperty(AgentIdKey, agentId);
    }

    /// <summary>
    /// 获取消息应用标识
    /// </summary>
    /// <param name="notification"></param>
    public static string GetAgentIdOrNull(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(AgentIdKey, out var agentIdDefine))
        {
            return agentIdDefine.ToString();
        }

        return null;
    }

    /// <summary>
    /// 指定接收消息的标签，标签ID列表，多个接收者用‘|’分隔，最多支持100个。
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static NotificationDefinition WithTag(
        this NotificationDefinition notification,
        string tag)
    {
        return notification.WithProperty(ToTagKey, tag);
    }

    /// <summary>
    /// 获取接收消息的标签
    /// </summary>
    /// <param name="notification"></param>
    public static string GetTagOrNull(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(ToTagKey, out var tagDefine))
        {
            return tagDefine.ToString();
        }

        return null;
    }

    /// <summary>
    /// 指定接收消息的部门，部门ID列表，多个接收者用‘|’分隔，最多支持100个。
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="party"></param>
    /// <returns></returns>
    public static NotificationDefinition WithParty(
        this NotificationDefinition notification,
        string party)
    {
        return notification.WithProperty(ToPartyKey, party);
    }

    /// <summary>
    /// 获取接收消息的部门
    /// </summary>
    /// <param name="notification"></param>
    public static string GetPartyOrNull(
        this NotificationDefinition notification)
    {
        if (notification.Properties.TryGetValue(ToPartyKey, out var partyDefine))
        {
            return partyDefine.ToString();
        }

        return null;
    }
}
