using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Sms
{
    /// <summary>
    /// 短信通知发送接口
    /// </summary>
    /// <remarks>
    /// 重写实现自定义的短信消息处理
    /// </remarks>
    public interface ISmsNotificationSender
    {
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="notification">通知数据</param>
        /// <param name="phoneNumbers">手机号列表,多个手机号通过,分隔</param>
        /// <returns></returns>
        Task SendAsync(NotificationInfo notification, string phoneNumbers);
    }
}
