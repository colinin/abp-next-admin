namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 消息系统错误码设计
    /// 状态码分为两部分 前2位领域 后3位状态
    /// 
    /// <list type="table">
    /// 领域部分：
    ///     01  输入
    ///     02  群组
    ///     03  用户
    ///     04  应用
    ///     05  内部
    ///     10  输出
    /// </list>
    /// 
    /// <list type="table">
    /// 状态部分：
    ///     200-299 成功
    ///     300-399 成功但有后续操作
    ///     400-499 业务异常
    ///     500-599 内部异常
    ///     900-999 输入输出异常
    /// </list>
    /// 
    /// </summary>
    public class NotificationsErrorCodes
    {
        public const string Namespace = "LINGYUN.Abp.Notifications";
        /// <summary>
        /// 通知模板不存在!
        /// </summary>
        public const string NotificationTemplateNotFound = Namespace + ":01404";
    }
}
