using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MicroService.MessageService.DataSeeds;

public class NotificationDataSeeder : ITransientDependency
{
    public ILogger<NotificationDataSeeder> Logger { protected get; set; }
    protected INotificationSender NotificationSender { get; }
    protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }
    public NotificationDataSeeder(
        INotificationSender notificationSender,
        INotificationSubscriptionManager notificationSubscriptionManager)
    {
        NotificationSender = notificationSender;
        NotificationSubscriptionManager = notificationSubscriptionManager;

        Logger = NullLogger<NotificationDataSeeder>.Instance;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await SubscribeDefaultNotifierAsync(context);
        await SeedWelcomeNotifierAsync(context);
    }

    public async virtual Task SubscribeDefaultNotifierAsync(DataSeedContext context)
    {
        if (!context.Properties.TryGetValue(MessageServiceDataSeedConsts.AdminUserIdPropertyName, out var userIdString) ||
            !Guid.TryParse(userIdString?.ToString(), out var adminUserId))
        {

            return;
        }
        var adminUserName = context.Properties.GetOrDefault(MessageServiceDataSeedConsts.AdminUserNamePropertyName)?.ToString() 
            ?? MessageServiceDataSeedConsts.AdminUserNameDefaultValue;
        var userIdentifer = new UserIdentifier(adminUserId, adminUserName);
        // 订阅内置通知
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                DefaultNotifications.SystemNotice);
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                DefaultNotifications.OnsideNotice);
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                DefaultNotifications.ActivityNotice);
        // 订阅用户欢迎消息
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                UserNotificationNames.WelcomeToApplication);
    }

    public async virtual Task SeedWelcomeNotifierAsync(DataSeedContext context)
    {
        try
        {
            if (!context.TenantId.HasValue)
            {
                return;
            }
            if (!context.Properties.TryGetValue(MessageServiceDataSeedConsts.AdminUserIdPropertyName, out var userIdString) ||
                !Guid.TryParse(userIdString?.ToString(), out var adminUserId))
            {
                return;
            }
            var adminEmailAddress = context.Properties.GetOrDefault(MessageServiceDataSeedConsts.AdminEmailPropertyName)?.ToString() 
                ?? MessageServiceDataSeedConsts.AdminEmailDefaultValue;
            var adminUserName = context.Properties.GetOrDefault(MessageServiceDataSeedConsts.AdminUserNamePropertyName)?.ToString() 
                ?? MessageServiceDataSeedConsts.AdminUserNameDefaultValue;

            var tenantAdminUserIdentifier = new UserIdentifier(adminUserId, adminEmailAddress);

            // 租户管理员订阅事件
            await NotificationSubscriptionManager
                .SubscribeAsync(
                    context.TenantId,
                    tenantAdminUserIdentifier,
                    TenantNotificationNames.NewTenantRegistered);

            Logger.LogInformation("publish new tenant notification..");
            await NotificationSender.SendNofiterAsync(
                TenantNotificationNames.NewTenantRegistered,
                new NotificationTemplate(
                    TenantNotificationNames.NewTenantRegistered,
                    formUser: adminEmailAddress,
                    data: new Dictionary<string, object>
                    {
                        { "name", adminUserName },
                        { "email", adminEmailAddress },
                        { "id", context.TenantId },
                    }),
                tenantAdminUserIdentifier,
                context.TenantId,
                NotificationSeverity.Success);

            Logger.LogInformation("tenant administrator subscribes to new tenant events..");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to send the tenant initialization notification.");
        }
    }
}
