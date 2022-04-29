using LINGYUN.Abp.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications.Emailing;

public class EmailingNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.Emailing;

    public override string Name => ProviderName;

    protected IEmailSender EmailSender { get; }

    protected ITemplateRenderer TemplateRenderer { get; }

    protected IStringLocalizerFactory LocalizerFactory { get; }

    protected IIdentityUserRepository UserRepository { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public EmailingNotificationPublishProvider(
        IServiceProvider serviceProvider,
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizerFactory localizerFactory,
        IIdentityUserRepository userRepository,
        INotificationDefinitionManager notificationDefinitionManager)
        : base(serviceProvider)
    {
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;
        LocalizerFactory = localizerFactory;
        UserRepository = userRepository;
        NotificationDefinitionManager = notificationDefinitionManager;
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

        var notificationDefinition = NotificationDefinitionManager.Get(notification.Name);
        var notificationDisplayName = notificationDefinition.DisplayName.Localize(LocalizerFactory).Value;

        notificationDefinition.Properties.TryGetValue("Template", out var template);
        if (template == null)
        {
            Logger.LogWarning("The email template is not specified, so the email notification cannot be sent!");
            return;
        }

        var content = await TemplateRenderer.RenderAsync(
                template.ToString(),
                globalContext: notification.Data.ExtraProperties);

        await EmailSender.SendAsync(emailAddress, notificationDisplayName, content);
    }
}
