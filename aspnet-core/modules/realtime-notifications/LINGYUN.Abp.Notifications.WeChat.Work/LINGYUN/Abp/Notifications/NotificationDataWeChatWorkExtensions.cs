namespace LINGYUN.Abp.Notifications;
public static class NotificationDataWeChatWorkExtensions
{
    private const string Prefix = "wx-work:";
    private const string AgentIdKey = Prefix + "agent_id";
    private const string ToTagKey = Prefix + "to_tag";
    private const string ToPartyKey = Prefix + "to_party";

    /// <summary>
    /// 设定发送到所有应用
    /// </summary>
    /// <param name="notificationData"></param>
    /// <returns></returns>
    public static void WithAllAgent(
        this NotificationData notificationData)
    {
        notificationData.SetAgentId("@all");
    }

    /// <summary>
    /// 设定消息应用标识
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="agentId"></param>
    /// <returns></returns>
    public static void SetAgentId(
        this NotificationData notificationData,
        string agentId)
    {
        notificationData.TrySetData(AgentIdKey, agentId);
    }
    /// <summary>
    /// 获取消息应用标识
    /// </summary>
    /// <param name="notificationData"></param>
    public static string GetAgentIdOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(AgentIdKey)?.ToString();
    }
    /// <summary>
    /// 指定接收消息的标签，标签ID列表，多个接收者用‘|’分隔，最多支持100个。
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static void SetTag(
        this NotificationData notificationData,
        string tag)
    {
        notificationData.TrySetData(ToTagKey, tag);
    }
    /// <summary>
    /// 获取接收消息的标签
    /// </summary>
    /// <param name="notificationData"></param>
    public static string GetTagOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(ToTagKey)?.ToString();
    }
    /// <summary>
    /// 指定接收消息的部门，部门ID列表，多个接收者用‘|’分隔，最多支持100个。
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="party"></param>
    /// <returns></returns>
    public static void SetParty(
        this NotificationData notificationData,
        string party)
    {
        notificationData.TrySetData(ToPartyKey, party);
    }
    /// <summary>
    /// 获取接收消息的部门
    /// </summary>
    /// <param name="notificationData"></param>
    public static string GetPartyOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(ToPartyKey)?.ToString();
    }
}
