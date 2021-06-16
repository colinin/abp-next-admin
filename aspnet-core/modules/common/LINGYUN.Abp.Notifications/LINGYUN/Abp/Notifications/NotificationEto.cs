using LINGYUN.Abp.RealTime;
using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications
{
    [Serializable]
    [GenericEventName(Prefix = "abp.realtime.")]
    public class NotificationEto<T> : RealTimeEto<T>, IMultiTenant
    {
        /// <summary>
        /// 通知标识
        /// 自动计算
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 租户
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 通知名称
        /// </summary>
        public string Name { get; set; }
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
        public NotificationEto() : base()
        {
        }

        public NotificationEto(T data) : base(data)
        {
        }
    }
}
