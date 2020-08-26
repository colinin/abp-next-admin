using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

/*
 * 通知系统的设计不应该定死通知名称
 * 而是规范通知的一些属性,因此不应该是自定义通知名称,而是定义通知的类目,类似于Catalog
 * 或者Prefix
 * 
 * TODO: 2020-08-26 如果需要用户或者租户特定的消息该如何来发送通知?
 *      是否追加字段：通知类别（宿主、租户、用户、通用）,主要可以在运行时判断发布消息的来源，
 *      如果是用户通知（NotificationData[FormUser]）则只会查询用户对于用户通知的订阅（用户互动：站内信、私信、好友请求、留言等），优先级最低
 *      租户通知（NotificationData[FormTenant]）则只会查询用户对于租户通知的订阅（系统发布、应用通知），优先级次于用户
 *      全局通知（NotificationData[FormGlobal]）则查询用户对于全局通知的订阅（一般用于系统发布、应用通知），优先级最高
 */

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDefinition
    {
        /// <summary>
        /// 通知类目
        /// </summary>
        [NotNull]
        public string CateGory { get; set; }
        /// <summary>
        /// 通知显示名称
        /// </summary>
        [NotNull]
        public ILocalizableString DisplayName
        {
            get => _displayName;
            set => _displayName = Check.NotNull(value, nameof(value));
        }
        private ILocalizableString _displayName;
        /// <summary>
        /// 通知说明
        /// </summary>
        [CanBeNull]
        public ILocalizableString Description { get; set; }
        /// <summary>
        /// 允许客户端显示订阅
        /// </summary>
        public bool AllowSubscriptionToClients { get; set; }
        /// <summary>
        /// 存活类型
        /// </summary>
        public NotificationLifetime NotificationLifetime { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// 通知提供者
        /// </summary>
        public List<string> Providers { get; }

        public NotificationDefinition(
           string category,
           ILocalizableString displayName = null,
           ILocalizableString description = null,
           NotificationType notificationType = NotificationType.Application,
           NotificationLifetime lifetime = NotificationLifetime.Persistent,
           bool allowSubscriptionToClients = false)
        {
            CateGory = category;
            DisplayName = displayName ?? new FixedLocalizableString(category);
            Description = description;
            NotificationLifetime = lifetime;
            NotificationType = notificationType;
            AllowSubscriptionToClients = allowSubscriptionToClients;

            Providers = new List<string>();
        }

        public virtual NotificationDefinition WithProviders(params string[] providers)
        {
            if (!providers.IsNullOrEmpty())
            {
                Providers.AddRange(providers);
            }

            return this;
        }
    }
}
