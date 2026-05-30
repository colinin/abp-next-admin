using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Identity.Notifications.Templates;

public class IdentityNotificationTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition(
                IdentityNotificationNames.IdentityUser.InactiveUserReminderNotifier,
                displayName: L("InactiveUserReminderNotifier"),
                localizationResource: typeof(IdentityResource)
            ).WithVirtualFilePath("/LINGYUN/Abp/Identity/Notifications/Templates/InactiveUserReminderNotifier.tpl", true),
            new TemplateDefinition(
                IdentityNotificationNames.IdentityUser.InactiveUserDeactivationNotifier,
                displayName: L("InactiveUserDeactivationNotifier"),
                localizationResource: typeof(IdentityResource)
            ).WithVirtualFilePath("/LINGYUN/Abp/Identity/Notifications/Templates/InactiveUserDeactivationNotifier.tpl", true),
            new TemplateDefinition(
                IdentityNotificationNames.IdentityUser.InactiveUserDeletionNotifier,
                displayName: L("InactiveUserDeletionNotifier"),
                localizationResource: typeof(IdentityResource)
            ).WithVirtualFilePath("/LINGYUN/Abp/Identity/Notifications/Templates/InactiveUserDeletionNotifier.tpl", true)
        );
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
