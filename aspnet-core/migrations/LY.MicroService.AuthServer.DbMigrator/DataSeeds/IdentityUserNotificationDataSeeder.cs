using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LY.MicroService.AuthServer.DbMigrator.DataSeeds;

public class IdentityUserNotificationDataSeeder : ITransientDependency
{
    public ILogger<IdentityUserNotificationDataSeeder> Logger { protected get; set; }
    protected IdentityUserManager IdentityUserManager { get; }
    protected INotificationSender NotificationSender { get; }
    protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }
    public IdentityUserNotificationDataSeeder(
        IdentityUserManager identityUserManager,
        INotificationSender notificationSender,
        INotificationSubscriptionManager notificationSubscriptionManager)
    {
        IdentityUserManager = identityUserManager;
        NotificationSender = notificationSender;
        NotificationSubscriptionManager = notificationSubscriptionManager;

        Logger = NullLogger<IdentityUserNotificationDataSeeder>.Instance;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await SubscribeDefaultNotifierAsync(context);
    }

    protected async virtual Task SubscribeDefaultNotifierAsync(DataSeedContext context)
    {
        var user = await IdentityUserManager.FindByNameAsync("admin");
        if (user == null)
        {
            Logger.LogInformation("No user information named {UserName} was found, so default messages cannot be subscribed to.", "admin");
            return;
        }
        Logger.LogInformation("User {UserId} has subscribed to default notifications.", user.Id);
        var userIdentifer = new UserIdentifier(user.Id, user.UserName);
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

        // 新用户订阅会话过期通知
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                IdentityNotificationNames.Session.ExpirationSession);
        // 新用户订阅不活跃用户相关通知
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                IdentityNotificationNames.IdentityUser.InactiveUserReminderNotifier);
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                IdentityNotificationNames.IdentityUser.InactiveUserDeactivationNotifier);

        // 订阅用户欢迎消息
        await NotificationSubscriptionManager
            .SubscribeAsync(
                context.TenantId,
                userIdentifer,
                UserNotificationNames.WelcomeToApplication);

        Logger.LogInformation("User {UserId} is subscribed to notifications by default.", user.Id);
    }
}
