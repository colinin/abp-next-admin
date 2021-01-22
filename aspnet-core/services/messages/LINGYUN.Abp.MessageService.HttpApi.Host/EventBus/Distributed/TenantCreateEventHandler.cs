using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MultiTenancy;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.MessageService.EventBus.Distributed
{
    public class TenantCreateEventHandler : IDistributedEventHandler<CreateEventData>, ITransientDependency
    {
        protected ILogger<TenantCreateEventHandler> Logger { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ISettingProvider SettingProvider { get; }
        protected IStringLocalizer StringLocalizer { get; }
        protected INotificationSender NotificationSender { get; }
        protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }

        public TenantCreateEventHandler(
            ICurrentTenant currentTenant,
            ISettingProvider settingProvider,
            INotificationSender notificationSender,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IStringLocalizer<MessageServiceResource> stringLocalizer,
            ILogger<TenantCreateEventHandler> logger)
        {
            Logger = logger;
            CurrentTenant = currentTenant;
            SettingProvider = settingProvider;
            StringLocalizer = stringLocalizer;
            NotificationSender = notificationSender;
            NotificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task HandleEventAsync(CreateEventData eventData)
        {
            var tenantAdminUserIdentifier = new UserIdentifier(eventData.AdminUserId, eventData.AdminEmailAddress);

            // 租户管理员订阅事件
            await NotificationSubscriptionManager
                .SubscribeAsync(
                    eventData.Id,
                    tenantAdminUserIdentifier,
                    TenantNotificationNames.NewTenantRegistered);

            var userDefaultCultureName = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
            if (userDefaultCultureName.IsNullOrWhiteSpace())
            {
                userDefaultCultureName = CultureInfo.CurrentUICulture.Name;
            }
            // 使用系统区域语言发布通知
            using (CultureHelper.Use(userDefaultCultureName, userDefaultCultureName))
            {
                var notificationData = new NotificationData();
                notificationData.WriteStandardData(
                    L("NewTenantRegisteredNotificationTitle"),
                    L("NewTenantRegisteredNotificationMessage", eventData.Name),
                    DateTime.Now, eventData.AdminEmailAddress);

                // 发布租户创建通知
                await NotificationSender
                    .SendNofiterAsync(
                        TenantNotificationNames.NewTenantRegistered, 
                        notificationData,
                        tenantAdminUserIdentifier,
                        eventData.Id, 
                        NotificationSeverity.Success);
            }
        }

        protected string L(string name, params object[] args)
        {
            return StringLocalizer[name, args]?.Value;
        }
    }
}
