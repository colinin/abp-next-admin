using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;

public class NotificationsCommonTemplateProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(CreateEmailTemplate());
    }

    protected virtual TemplateDefinition[] CreateEmailTemplate()
    {
        return new TemplateDefinition[]
        {
            new TemplateDefinition(
                name: "EmailNotifierLayout",
                displayName: L("EmailNotifierLayout"),
                defaultCultureName: "en",
                isLayout: true)
                .WithVirtualFilePath( "/LINGYUN/Abp/Notifications/Templates/layout.tpl", isInlineLocalized: true)
        };
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<NotificationsResource>(name);
    }
}
