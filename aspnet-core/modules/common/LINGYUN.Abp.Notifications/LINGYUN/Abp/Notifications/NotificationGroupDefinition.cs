using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationGroupDefinition
    {
        /// <summary>
        /// 通知组名称
        /// </summary>
        [NotNull]
        public string Name { get; set; }
        /// <summary>
        /// 通知组显示名称
        /// </summary>
        [NotNull]
        public ILocalizableString DisplayName
        {
            get => _displayName;
            set => _displayName = Check.NotNull(value, nameof(value));
        }
        private ILocalizableString _displayName;
        /// <summary>
        /// 通知组说明
        /// </summary>
        [CanBeNull]
        public ILocalizableString Description { get; set; }
        public bool AllowSubscriptionToClients { get; set; }
        public IReadOnlyList<NotificationDefinition> Notifications => _notifications.ToImmutableList();
        private readonly List<NotificationDefinition> _notifications;

        protected internal NotificationGroupDefinition(
            string name,
            ILocalizableString displayName = null,
            bool allowSubscriptionToClients = false)
        {
            Name = name;
            DisplayName = displayName ?? new FixedLocalizableString(Name);
            AllowSubscriptionToClients = allowSubscriptionToClients;

            _notifications = new List<NotificationDefinition>();
        }

        public virtual NotificationDefinition AddNotification(
            string name,
           ILocalizableString displayName = null,
           ILocalizableString description = null,
           NotificationType notificationType = NotificationType.Application,
           NotificationLifetime lifetime = NotificationLifetime.Persistent,
           bool allowSubscriptionToClients = false)
        {
            var notification = new NotificationDefinition(
                name,
                displayName,
                description,
                notificationType,
                lifetime,
                allowSubscriptionToClients
            );

            _notifications.Add(notification);

            return notification;
        }
    }
}
