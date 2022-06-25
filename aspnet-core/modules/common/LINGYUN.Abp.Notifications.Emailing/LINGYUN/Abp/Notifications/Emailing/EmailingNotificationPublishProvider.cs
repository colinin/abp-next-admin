using LINGYUN.Abp.Identity;
using LINGYUN.Abp.RealTime.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications.Emailing;

public class EmailingNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.Emailing;

    public override string Name => ProviderName;

    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected IEmailSender EmailSender { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected IIdentityUserRepository UserRepository { get; }

    public EmailingNotificationPublishProvider(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizerFactory localizerFactory,
        IIdentityUserRepository userRepository,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        EmailSender = emailSender;
        LocalizerFactory = localizerFactory;
        UserRepository = userRepository;

        LocalizationOptions = localizationOptions.Value;
    }

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

        if (!notification.Data.NeedLocalizer())
        {
            var title = notification.Data.TryGetData("title").ToString();
            var message = notification.Data.TryGetData("message").ToString();

            await EmailSender.SendAsync(emailAddress, title, message);
        }
        else
        {
            var titleInfo = notification.Data.TryGetData("title").As<LocalizableStringInfo>();
            var titleResource = GetResource(titleInfo.ResourceName);
            var title = LocalizerFactory.Create(titleResource.ResourceType)[titleInfo.Name, titleInfo.Values].Value;

            var messageInfo = notification.Data.TryGetData("message").As<LocalizableStringInfo>();
            var messageResource = GetResource(messageInfo.ResourceName);
            var message = LocalizerFactory.Create(messageResource.ResourceType)[messageInfo.Name, messageInfo.Values].Value;

            await EmailSender.SendAsync(emailAddress, title, message);
        }
    }

    private LocalizationResource GetResource(string resourceName)
    {
        return LocalizationOptions.Resources.Values
            .First(x => x.ResourceName.Equals(resourceName));
    }
}
