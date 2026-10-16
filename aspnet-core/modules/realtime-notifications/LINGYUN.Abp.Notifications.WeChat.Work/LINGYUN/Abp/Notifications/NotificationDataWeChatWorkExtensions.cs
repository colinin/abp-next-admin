using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications;
public static partial class NotificationDataWeChatWorkExtensions
{
    private const string Prefix = "wx-work:";
    private const string AgentIdKey = Prefix + "agent_id";
    private const string ToTagKey = Prefix + "to_tag";
    private const string ToPartyKey = Prefix + "to_party";
    private const string SafeKey = Prefix + "safe";
    private const string EnableIdTransKey = Prefix + "enable_id_trans";
    private const string EnableDuplicateCheckKey = Prefix + "enable_duplicate_check";
    private const string DuplicateCheckIntervalKey = Prefix + "duplicate_check_interval";

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
    public static string? GetAgentIdOrNull(
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
    public static string? GetTagOrNull(
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
    public static string? GetPartyOrNull(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(ToPartyKey)?.ToString();
    }

    /// <summary>
    /// 设置是否保密消息(仅文本消息支持)
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="safe"></param>
    public static void SetSafe(
        this NotificationData notificationData,
        bool safe)
    {
        notificationData.TrySetData(SafeKey, safe ? 1 : 0);
    }

    /// <summary>
    /// 获取是否保密消息, 默认 0(不保密)
    /// </summary>
    /// <remarks>
    /// 企业微信要求 int: 0 表示否, 1 表示是
    /// </remarks>
    /// <param name="notificationData"></param>
    public static int GetSafe(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(SafeKey) is bool safe
            ? (safe ? 1 : 0)
            : 0;
    }

    /// <summary>
    /// 设置是否开启id转译
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="enableIdTrans"></param>
    public static void SetEnableIdTrans(
        this NotificationData notificationData,
        bool enableIdTrans)
    {
        notificationData.TrySetData(EnableIdTransKey, enableIdTrans ? (byte)1 : (byte)0);
    }

    /// <summary>
    /// 获取是否开启id转译, 默认 0(不开启)
    /// </summary>
    /// <remarks>
    /// 企业微信要求 byte: 0 表示否, 1 表示是
    /// </remarks>
    /// <param name="notificationData"></param>
    public static byte GetEnableIdTrans(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(EnableIdTransKey) is bool enable
            ? (enable ? (byte)1 : (byte)0)
            : (byte)0;
    }

    /// <summary>
    /// 设置是否开启重复消息检查
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="enableDuplicateCheck"></param>
    public static void SetEnableDuplicateCheck(
        this NotificationData notificationData,
        bool enableDuplicateCheck)
    {
        notificationData.TrySetData(EnableDuplicateCheckKey, enableDuplicateCheck ? (byte)1 : (byte)0);
    }

    /// <summary>
    /// 获取是否开启重复消息检查, 默认 0(不开启)
    /// </summary>
    /// <remarks>
    /// 企业微信要求 byte: 0 表示否, 1 表示是
    /// </remarks>
    /// <param name="notificationData"></param>
    public static byte GetEnableDuplicateCheck(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(EnableDuplicateCheckKey) is bool enable
            ? (enable ? (byte)1 : (byte)0)
            : (byte)0;
    }

    /// <summary>
    /// 设置重复消息检查的时间间隔(秒)
    /// </summary>
    /// <param name="notificationData"></param>
    /// <param name="interval"></param>
    public static void SetDuplicateCheckInterval(
        this NotificationData notificationData,
        int interval)
    {
        notificationData.TrySetData(DuplicateCheckIntervalKey, interval);
    }

    /// <summary>
    /// 获取重复消息检查的时间间隔(秒), 默认 1800
    /// </summary>
    /// <param name="notificationData"></param>
    public static int GetDuplicateCheckInterval(
        this NotificationData notificationData)
    {
        return notificationData.TryGetData(DuplicateCheckIntervalKey) is int interval
            ? interval
            : 1800;
    }

    /// <summary>
    /// 尝试获取消息载荷
    /// </summary>
    /// <remarks>
    /// 消息载荷以 JSON 字符串写入属性包(见各 <c>With*Message</c> 方法),
    /// 因此这里按 JSON 反序列化还原。<c>TMessage</c> 若为具体类型可直接还原;
    /// 抽象类型(如 <c>TemplateCard</c>)需要 JSON 转换器参与类型判别。
    /// </remarks>
    /// <param name="notificationData"></param>
    /// <param name="messageKey">消息键</param>
    /// <param name="message">消息载荷</param>
    /// <returns></returns>
    public static bool TryGetMessagePayload<TMessage>(
        this NotificationData notificationData,
        string messageKey,
        out TMessage? message)
        where TMessage : class
    {
        message = null;

        var payload = notificationData.TryGetData(messageKey);
        if (payload == null)
        {
            return false;
        }

        // 属性包中的值可能是 JSON 字符串, 也可能已被反序列化为 JSON 节点
        var json = payload as string ?? payload.ToString();

        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        try
        {
            message = JsonConvert.DeserializeObject<TMessage>(json, JsonSettings);
        }
        catch (JsonException)
        {
            // 载荷非法时返回 false, 由调用方决定是否回退
            return false;
        }

        return message != null;
    }

    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        Converters = { new TemplateCardJsonConverter() }
    };
}
