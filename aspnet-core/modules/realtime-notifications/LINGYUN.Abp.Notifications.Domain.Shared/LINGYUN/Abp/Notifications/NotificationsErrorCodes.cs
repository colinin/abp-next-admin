namespace LINGYUN.Abp.Notifications;

/// <summary>
/// 消息系统错误码设计
/// 状态码分为两部分 前2位领域 后3位状态  
/// <br />
/// 领域部分：
/// <list type="table">
///     <item>000-019  输入</item>
///     <item>020-029  群组</item>
///     <item>030-039  用户</item>
///     <item>040-049  应用</item>
///     <item>050-059  内部</item>
///     <item>100-199  输出</item>
/// </list>
/// 
/// 状态部分：
/// <list type="table">
///     <item>200-299 成功</item>
///     <item>300-399 成功但有后续操作</item>
///     <item>400-499 业务异常</item>
///     <item>500-599 内部异常</item>
///     <item>900-999 输入输出异常</item>
/// </list>
/// 
/// </summary>
public class NotificationsErrorCodes
{
    public const string Namespace = "Notifications";
    /// <summary>
    /// 通知模板不存在!
    /// </summary>
    public const string NotificationTemplateNotFound = Namespace + ":001404";

    public static class GroupDefinition
    {
        private const string Prefix = Namespace + ":002";

        public const string StaticGroupNotAllowedChanged = Prefix + "400";

        public const string AlreayNameExists = Prefix + "403";

        public const string NameNotFount = Prefix + "404";
    }

    public static class Definition
    {
        private const string Prefix = Namespace + ":003";

        public const string StaticFeatureNotAllowedChanged = Prefix + "400";

        public const string FailedGetGroup = Prefix + "401";

        public const string AlreayNameExists = Prefix + "403";

        public const string NameNotFount = Prefix + "404";
    }
}
