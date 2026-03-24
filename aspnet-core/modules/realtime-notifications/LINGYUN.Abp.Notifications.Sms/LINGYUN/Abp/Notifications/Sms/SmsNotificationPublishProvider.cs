using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Sms;

public class SmsNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.Sms;
    public override string Name => ProviderName;
    protected IUserPhoneFinder UserPhoneFinder => ServiceProvider.LazyGetRequiredService<IUserPhoneFinder>();
    protected ISmsNotificationSender Sender => ServiceProvider.LazyGetRequiredService<ISmsNotificationSender>();
    protected IOptions<AbpNotificationsSmsOptions> Options => ServiceProvider.LazyGetRequiredService<IOptions<AbpNotificationsSmsOptions>>();

    protected override async Task PublishAsync(
        NotificationPublishContext context,
        CancellationToken cancellationToken = default)
    {
        if (!context.Users.Any())
        {
            context.Cancel("The user who received the text message is empty.");
            return;
        }

        var sendToPhones = await UserPhoneFinder.FindByUserIdsAsync(context.Users.Select(usr => usr.UserId), cancellationToken);
        if (!sendToPhones.Any())
        {
            context.Cancel("The user has not confirmed their mobile phone number, so the message cannot be sent.");
            return;
        }
        await Sender.SendAsync(context.Notification, sendToPhones.JoinAsString(","));

        Logger.LogDebug("The notification: {0} with provider: {1} has successfully published!", context.Notification.Name, Name);
    }
}
