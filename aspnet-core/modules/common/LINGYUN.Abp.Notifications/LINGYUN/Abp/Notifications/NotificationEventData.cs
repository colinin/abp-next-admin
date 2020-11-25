using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationEventData : IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 通知名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用来标识一个应用程序
        /// </summary>
        /// <remarks>
        /// tips: 可以通过它来特定于应用程序的边界
        /// </remarks>
        public string Application { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public NotificationData Data { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 紧急级别
        /// </summary>
        public NotificationSeverity Severity { get; set; }
        /// <summary>
        /// 指定的接收用户信息集合
        /// </summary>
        /// <remarks>
        /// 注:<br/>
        /// 如果指定了用户列表,应该在事件订阅程序中通过此集合过滤订阅用户<br/>
        /// 如果未指定用户列表,应该在事件订阅程序中过滤所有订阅此通知的用户
        /// </remarks>
        public List<UserIdentifier> Users { get; set; }
        public NotificationEventData()
        {
            Application = "Abp";

            Users = new List<UserIdentifier>();
        }
    }
}
