using LINGYUN.Abp.Notifications;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationDto
    {
        /// <summary>
        /// 通知名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 存活类型
        /// </summary>
        public NotificationLifetime Lifetime { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public NotificationType Type { get; set; }
    }
}
