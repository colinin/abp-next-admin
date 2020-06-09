using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDefinition
    {
        /// <summary>
        /// 通知名称
        /// </summary>
        [NotNull]
        public string Name { get; set; }
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
        /// 通知类型
        /// </summary>
        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// 通知提供者
        /// </summary>
        public List<string> Providers { get; }

        public NotificationDefinition(
           string name,
           ILocalizableString displayName = null,
           ILocalizableString description = null,
           NotificationType notificationType = NotificationType.Application,
           bool allowSubscriptionToClients = false)
        {
            Name = name;
            DisplayName = displayName ?? new FixedLocalizableString(name);
            Description = description;
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
