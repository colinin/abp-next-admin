using LINGYUN.Abp.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Notifications.Emailing;

public class EmailingNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.Emailing;
    public override string Name => ProviderName;
    protected IEmailSender EmailSender => ServiceProvider.LazyGetRequiredService<IEmailSender>();
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected IIdentityUserRepository UserRepository => ServiceProvider.LazyGetRequiredService<IIdentityUserRepository>();
    protected INotificationDataSerializer NotificationDataSerializer => ServiceProvider.LazyGetRequiredService<INotificationDataSerializer>();

    protected async override Task PublishAsync(
        NotificationInfo notification,
        IEnumerable<UserIdentifier> identifiers,
        CancellationToken cancellationToken = default)
    {
        var userIds = identifiers.Select(x => x.UserId).ToList();
        var userList = await UserRepository.GetListByIdListAsync(userIds, cancellationToken: cancellationToken);
        var emailAddress = userList.Where(x => x.EmailConfirmed).Select(x => x.Email).Distinct().JoinAsString(",");

        if (emailAddress.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("The subscriber did not confirm the email address and could not send email notifications!");
            return;
        }
        var notificationData = await NotificationDataSerializer.ToStandard(notification.Data);

        await EmailSender.SendAsync(emailAddress, notificationData.Title, notificationData.Message);
    }
}
