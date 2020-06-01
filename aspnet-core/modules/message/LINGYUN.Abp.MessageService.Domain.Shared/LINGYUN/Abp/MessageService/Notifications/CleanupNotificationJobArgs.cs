using System;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class CleanupNotificationJobArgs
    {
        /// <summary>
        /// 清理大小
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 清理租户
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
